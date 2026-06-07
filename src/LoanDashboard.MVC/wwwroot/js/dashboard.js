/* ==========================================================================
   Loan Dashboard — daily counts with Partner filtering
   ========================================================================== */
(() => {
  'use strict';

  const STAGE_COLORS = [
    '#6366f1', '#8b5cf6', '#a855f7', '#ec4899',
    '#f43f5e', '#f97316', '#eab308', '#22c55e',
    '#14b8a6', '#06b6d4', '#3b82f6', '#84cc16'
  ];

  const $ = id => document.getElementById(id);
  const els = {
    statusDot:      $('status-dot'),
    statusText:     $('status-text'),
    lastTick:       $('last-tick'),
    kpiTotal:       $('kpi-total'),
    kpiActive:      $('kpi-active'),
    stageGrid:      $('stage-grid'),
    bottleneck:     $('bottleneck'),
    bottleneckText: $('bottleneck-text'),
    chartCanvas:    $('stage-chart'),
    btnRefresh:     $('btn-refresh'),
    btnRefreshIcon: $('btn-refresh-icon'),
    btnRefreshText: $('btn-refresh-text'),
    partnerForm:    $('partner-filter-form'),
    partnerSelect:  $('partner-select'),
    btnSearch:      $('btn-search'),
    btnSearchIcon:  $('btn-search-icon'),
    btnSearchText:  $('btn-search-text'),
    btnSearchSpin:  $('btn-search-spinner'),
    filterLabel:    $('filter-status-label'),
    filterError:    $('filter-error')
  };

  let stages         = window.__INITIAL_STAGES__   || [];
  let partners       = window.__INITIAL_PARTNERS__ || [];
  let chart          = null;
  // Active partner filter: 0 = "All Partners" (SignalR live updates apply).
  // Any non-zero value pauses the SignalR-driven repaint so the user sees
  // only the partner they searched for.
  let activePartnerId = 0;

  /* ─── Status dot ─── */
  function setStatus(state, text) {
    els.statusDot.className = 'status-dot ' + state;
    els.statusText.textContent = text;
  }

  /* ─── KPIs ─── */
  function renderKpis() {
    const total  = stages.reduce((s, x) => s + x.count, 0);
    const active = stages.filter(s => s.count > 0).length;
    els.kpiTotal.textContent  = total;
    els.kpiActive.textContent = active;
  }

  /* ─── Stage cards ─── */
  function renderStageGrid() {
    if (!stages.length) {
      els.stageGrid.innerHTML = Array.from({ length: 12 },
        () => '<div class="col-md-3 col-lg-2"><div class="skeleton"></div></div>'
      ).join('');
      return;
    }

    els.stageGrid.innerHTML = stages.map((stage, i) => {
      const color  = STAGE_COLORS[i % STAGE_COLORS.length];
      const numCls = 'stage-count ' + (stage.count === 0 ? 'empty' : 'text-light');
      const time   = new Date(stage.lastUpdated).toLocaleTimeString();

      return `
        <div class="col-6 col-md-4 col-lg-3 col-xl-2">
          <div class="card stage-card border-secondary-subtle">
            <div class="stage-accent" style="background:${color}"></div>
            <div class="card-body d-flex flex-column">
              <div class="stage-name mb-2">${escapeHtml(stage.stageName)}</div>
              <div class="${numCls} my-2">${stage.count}</div>
              <div class="stage-time mt-auto">Updated ${time}</div>
            </div>
          </div>
        </div>`;
    }).join('');
  }

  /* ─── Bottleneck callout ─── */
  function renderBottleneck() {
    const top = stages.reduce((p, c) => c.count > p.count ? c : p,
                              { count: 0, stageName: '—' });
    if (top.count <= 0) {
      els.bottleneck.classList.add('d-none');
      return;
    }
    els.bottleneck.classList.remove('d-none');
    els.bottleneckText.innerHTML =
      `Busiest stage today: <strong class="text-light">${escapeHtml(top.stageName)}</strong> ` +
      `with <strong class="text-info">${top.count}</strong> applications.`;
  }

  /* ─── Chart ─── */
  function renderChart() {
    const labels   = stages.map(s =>
      s.stageName.length > 14 ? s.stageName.slice(0, 13) + '…' : s.stageName);
    const data     = stages.map(s => s.count);
    const bgColors = stages.map((_, i) => STAGE_COLORS[i % STAGE_COLORS.length]);

    if (chart) {
      chart.data.labels = labels;
      chart.data.datasets[0].data = data;
      chart.data.datasets[0].backgroundColor = bgColors;
      chart.update('none');
      return;
    }

    chart = new Chart(els.chartCanvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          data,
          backgroundColor: bgColors,
          borderRadius: 4,
          maxBarThickness: 40
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend:  { display: false },
          tooltip: {
            backgroundColor: '#1e2235',
            borderColor: '#2e3250',
            borderWidth: 1,
            padding: 10,
            titleColor: '#e2e8f0',
            bodyColor:  '#818cf8',
            callbacks: {
              title: items => stages[items[0].dataIndex].stageName,
              label: item  => `${stages[item.dataIndex].count} applications today`
            }
          }
        },
        scales: {
          x: {
            ticks: { color: '#8892b0', font: { size: 10 }, maxRotation: 40, minRotation: 40 },
            grid:  { display: false }
          },
          y: {
            beginAtZero: true,
            ticks: { color: '#8892b0', font: { size: 11 }, precision: 0 },
            grid:  { color: '#2e3250', drawBorder: false }
          }
        }
      }
    });
  }

  function renderAll() {
    renderKpis(); renderStageGrid(); renderBottleneck(); renderChart();
  }

  /* ─── Util ─── */
  function escapeHtml(str) {
    return String(str).replace(/[&<>"']/g, ch =>
      ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[ch]));
  }
  function tickStamp() {
    els.lastTick.textContent = 'Last update: ' + new Date().toLocaleTimeString();
  }
  function showFilterError(msg) {
    els.filterError.textContent = msg;
    els.filterError.classList.remove('d-none');
  }
  function clearFilterError() {
    els.filterError.textContent = '';
    els.filterError.classList.add('d-none');
  }

  /* ─── Partner dropdown (re-hydrate from the API in case it changed) ─── */
  async function loadPartners() {
    try {
      const res = await fetch('/api/stages/partners', { cache: 'no-store' });
      if (!res.ok) throw new Error('HTTP ' + res.status);
      partners = await res.json();
      const previous = els.partnerSelect.value;
      const opts = ['<option value="0">All Partners</option>']
        .concat(partners.map(p =>
          `<option value="${p.partnerId}">${escapeHtml(p.partnerName)}</option>`));
      els.partnerSelect.innerHTML = opts.join('');
      // Keep current selection if still available
      if ([...els.partnerSelect.options].some(o => o.value === previous)) {
        els.partnerSelect.value = previous;
      }
    } catch (err) {
      console.error('[Dashboard] partner load failed', err);
      // Server-rendered options remain usable.
    }
  }

  function setSearchBusy(busy) {
    els.btnSearch.disabled = busy;
    els.partnerSelect.disabled = busy;
    if (busy) {
      els.btnSearchSpin.classList.remove('d-none');
      els.btnSearchIcon.classList.add('d-none');
      els.btnSearchText.textContent = 'Loading…';
    } else {
      els.btnSearchSpin.classList.add('d-none');
      els.btnSearchIcon.classList.remove('d-none');
      els.btnSearchText.textContent = 'Search';
    }
  }

  /* ─── Search handler — applies the partner filter and reloads counts ─── */
  async function onSearch() {
    clearFilterError();
    const selectedId = parseInt(els.partnerSelect.value, 10) || 0;
    const selectedLabel = els.partnerSelect.options[els.partnerSelect.selectedIndex]?.text || 'All Partners';

    setSearchBusy(true);
    try {
      const url = selectedId > 0
        ? `/api/stages/count?partnerId=${encodeURIComponent(selectedId)}`
        : '/api/stages/count';
      const res = await fetch(url, { cache: 'no-store' });
      if (!res.ok) throw new Error('HTTP ' + res.status);
      stages = await res.json();
      activePartnerId = selectedId;
      els.filterLabel.textContent = selectedLabel;
      tickStamp();
      renderAll();
    } catch (err) {
      console.error('[Dashboard] partner search failed', err);
      showFilterError('Could not load counts for the selected partner. Please try again.');
    } finally {
      setSearchBusy(false);
    }
  }

  /* ─── SignalR ─── */
  function startSignalR() {
    const conn = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/dashboard')
      .withAutomaticReconnect([0, 1000, 3000, 5000, 10000])
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    conn.on('ReceiveStageCounts', data => {
      // Push from the poller is the all-partners view. Only apply it when the
      // user is currently viewing all partners — otherwise we'd overwrite the
      // partner-specific counts they searched for.
      if (activePartnerId !== 0) return;
      stages = data;
      tickStamp();
      renderAll();
    });
    conn.onreconnecting(() => setStatus('connecting', 'Reconnecting…'));
    conn.onreconnected(()  => setStatus('live',         'Live'));
    conn.onclose(()        => setStatus('disconnected', 'Disconnected'));

    conn.start()
      .then(() => setStatus('live', 'Live'))
      .catch(err => {
        console.error('[SignalR] connect failed', err);
        setStatus('disconnected', 'Disconnected');
        setTimeout(startSignalR, 5000);
      });
  }

  /* ─── Manual refresh button — honours the active partner filter ─── */
  async function manualRefresh() {
    if (els.btnRefresh.disabled) return;
    els.btnRefresh.disabled = true;
    els.btnRefreshIcon.classList.add('spin');
    els.btnRefreshText.textContent = 'Refreshing…';

    try {
      const url = activePartnerId > 0
        ? `/api/stages/count?partnerId=${encodeURIComponent(activePartnerId)}`
        : '/api/stages/count';
      const res = await fetch(url, { cache: 'no-store' });
      if (!res.ok) throw new Error('HTTP ' + res.status);
      stages = await res.json();
      tickStamp();
      renderAll();
    } catch (err) {
      console.error('[Dashboard] manual refresh failed', err);
      els.btnRefreshText.textContent = 'Retry';
      setTimeout(() => { els.btnRefreshText.textContent = 'Refresh'; }, 2000);
    } finally {
      els.btnRefreshIcon.classList.remove('spin');
      els.btnRefresh.disabled = false;
      if (els.btnRefreshText.textContent === 'Refreshing…')
        els.btnRefreshText.textContent = 'Refresh';
    }
  }

  /* ─── Boot ─── */
  document.addEventListener('DOMContentLoaded', () => {
    els.btnRefresh.addEventListener('click', manualRefresh);
    els.partnerForm.addEventListener('submit', e => { e.preventDefault(); onSearch(); });
    els.btnSearch.addEventListener('click',   e => { e.preventDefault(); onSearch(); });
    renderAll();
    loadPartners();
    startSignalR();
  });
})();
