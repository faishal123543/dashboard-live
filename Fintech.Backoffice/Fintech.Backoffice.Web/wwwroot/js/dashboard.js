// ============================================
// FinTech Dashboard - Charts & Auto-refresh
// ============================================

// Chart instances
let dailyChartInstance = null;
let subscriberChartInstance = null;
let distributionChartInstance = null;

// Chart.js global defaults for dark theme
if (typeof Chart !== 'undefined') {
    Chart.defaults.color = '#a8b2c9';
    Chart.defaults.borderColor = '#2a3550';
    Chart.defaults.font.family = "'Segoe UI', sans-serif";
}

function initializeAllCharts() {
    initializeDailyChart();
    initializeSubscriberChart();
    initializeDistributionChart();
}

// 1. Stacked Bar Chart - Applications Overview
function initializeDailyChart() {
    const ctx = document.getElementById('dailyChart');
    if (!ctx) return;

    dailyChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Oct', 'Nov', 'Dec'],
            datasets: [
                {
                    label: 'Completed',
                    data: [320, 450, 380],
                    backgroundColor: '#00d4ff',
                    borderRadius: 6,
                    stack: 'stack1'
                },
                {
                    label: 'In Progress',
                    data: [180, 210, 250],
                    backgroundColor: '#0099cc',
                    borderRadius: 6,
                    stack: 'stack1'
                },
                {
                    label: 'Rejected',
                    data: [120, 90, 110],
                    backgroundColor: '#7c3aed',
                    borderRadius: 6,
                    stack: 'stack1'
                },
                {
                    label: 'Cancelled',
                    data: [60, 40, 50],
                    backgroundColor: '#ffaa00',
                    borderRadius: 6,
                    stack: 'stack1'
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#1a1f2e',
                    titleColor: '#fff',
                    bodyColor: '#a8b2c9',
                    borderColor: '#2a3550',
                    borderWidth: 1,
                    padding: 12
                }
            },
            scales: {
                x: {
                    stacked: true,
                    grid: { display: false },
                    ticks: { color: '#a8b2c9' }
                },
                y: {
                    stacked: true,
                    grid: { color: 'rgba(42, 53, 80, 0.5)' },
                    ticks: { color: '#a8b2c9' }
                }
            }
        }
    });
}

// 2. Bar Chart - Total Subscribers
function initializeSubscriberChart() {
    const ctx = document.getElementById('subscriberChart');
    if (!ctx) return;

    subscriberChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
            datasets: [{
                label: 'Subscribers',
                data: [2400, 1800, 5200, 2100, 1900, 2300, 2800],
                backgroundColor: function (context) {
                    const index = context.dataIndex;
                    return index === 2 ? '#7c3aed' : '#3d4862';
                },
                borderRadius: 6,
                barPercentage: 0.6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#1a1f2e',
                    titleColor: '#fff',
                    bodyColor: '#a8b2c9'
                }
            },
            scales: {
                x: {
                    grid: { display: false },
                    ticks: { color: '#a8b2c9' }
                },
                y: {
                    display: false
                }
            }
        }
    });
}

// 3. Doughnut Chart - Sales Distribution
function initializeDistributionChart() {
    const ctx = document.getElementById('distributionChart');
    if (!ctx) return;

    distributionChartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Website', 'Mobile App', 'Other'],
            datasets: [{
                data: [60, 30, 10],
                backgroundColor: ['#00d4ff', '#7c3aed', '#ffaa00'],
                borderColor: '#1e2533',
                borderWidth: 3,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '70%',
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#1a1f2e',
                    titleColor: '#fff',
                    bodyColor: '#a8b2c9'
                }
            }
        }
    });
}

// Auto-refresh every 30 seconds
function startAutoRefresh() {
    setInterval(function () {
        refreshDashboardMetrics();
    }, 30000); // 30 seconds
}

function refreshDashboardMetrics() {
    fetch('/Dashboard/GetMetrics')
        .then(response => response.json())
        .then(data => {
            updateKpi('totalApplications', data.totalApplications.toLocaleString());
            updateKpi('inProgressCount', data.inProgress);
            updateKpi('completedCount', data.completed.toLocaleString());
            updateKpi('rejectedCount', data.rejected);
            updateKpi('cancelledCount', data.cancelled);
            updateKpi('todayApproved', '$' + data.todayApproved.toLocaleString());
            updateKpi('monthlyApproved', '$' + (data.monthlyApproved / 1000000).toFixed(2) + 'M');
            updateKpi('rejectionPct', data.rejectionPercentage + '%');

            const el = document.getElementById('lastUpdated');
            if (el) el.textContent = data.lastUpdated;
        })
        .catch(err => console.error('Refresh error:', err));
}

function updateKpi(id, value) {
    const el = document.getElementById(id);
    if (el && el.textContent !== value.toString()) {
        el.style.transition = 'all 0.3s';
        el.style.color = '#00d4ff';
        el.textContent = value;
        setTimeout(() => {
            el.style.color = '';
        }, 500);
    }
}

function updateLastUpdatedTime() {
    const el = document.getElementById('lastUpdated');
    if (el) {
        const now = new Date();
        el.textContent = now.toLocaleTimeString();
    }
}

// Mobile sidebar toggle + user menu dropdown
document.addEventListener('DOMContentLoaded', function () {
    const toggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');
    if (toggle && sidebar) {
        toggle.addEventListener('click', function () {
            sidebar.classList.toggle('show');
        });
    }

    // User menu dropdown
    const userToggle = document.getElementById('userMenuToggle');
    const userDropdown = document.getElementById('userMenuDropdown');
    if (userToggle && userDropdown) {
        userToggle.addEventListener('click', function (e) {
            e.stopPropagation();
            userDropdown.classList.toggle('show');
        });
        document.addEventListener('click', function (e) {
            if (!userDropdown.contains(e.target) && !userToggle.contains(e.target)) {
                userDropdown.classList.remove('show');
            }
        });
    }
});
