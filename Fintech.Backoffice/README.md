# FinTech Backoffice Dashboard Application

A modern, enterprise-grade analytics dashboard for FinTech operations built with ASP.NET Core MVC (.NET 9), Entity Framework Core, and SQL Server.

## 📊 Overview

This application provides operations teams with real-time visibility into loan and invoice applications with:
- **Real-time Dashboard KPIs** - Monitor applications by status and approved amounts
- **Advanced Filtering** - Search and filter by date, status, partner, and customer
- **Interactive Charts** - Daily trends, status distribution, partner performance
- **Data Export** - Export applications to Excel for reporting
- **Audit Trail** - Complete tracking of all changes
- **Role-based Access** - Admin, Operations Manager, and Analyst roles

## 🏗️ Architecture

```
Clean Architecture with 4-Layer Design:

┌─────────────────────────────────────────────────────────────┐
│  Fintech.Backoffice.Web (ASP.NET Core MVC)                 │
│  ├─ Controllers                                             │
│  ├─ Razor Views (Dark Theme)                               │
│  └─ wwwroot (CSS, JS, Static Files)                        │
├─────────────────────────────────────────────────────────────┤
│  Fintech.Backoffice.Application (Business Logic)           │
│  ├─ Services (IApplicationService, IDashboardService)      │
│  ├─ DTOs (Data Transfer Objects)                           │
│  ├─ ViewModels                                              │
│  ├─ Validators (FluentValidation)                          │
│  └─ Mappings (AutoMapper)                                  │
├─────────────────────────────────────────────────────────────┤
│  Fintech.Backoffice.Infrastructure (Data Access)           │
│  ├─ DbContext (Entity Framework)                           │
│  ├─ Repositories (Generic Repository Pattern)              │
│  ├─ Unit of Work Pattern                                   │
│  └─ Migrations                                             │
├─────────────────────────────────────────────────────────────┤
│  Fintech.Backoffice.Domain (Entities & Interfaces)         │
│  ├─ Entities (Application, Customer, Partner, AuditLog)    │
│  ├─ Enums (ApplicationStatus)                              │
│  └─ Interfaces (IRepository<T>, IUnitOfWork)               │
└─────────────────────────────────────────────────────────────┘
```

## 🎨 Features

### Dashboard (Real-time Metrics)
- Total Applications (All time, Today)
- Applications by Status (In-Progress, Rejected, Completed, Cancelled)
- Total Approved Amount (Daily, Monthly)
- Rejection Percentage
- Recent Applications Table
- Partner-wise Analytics

### Charts & Analytics
- Daily Application Trends (Line Chart)
- Status Distribution (Doughnut Chart)
- Rejected vs Completed (Pie Chart)
- Monthly Approved Amounts (Bar Chart)
- Partner Performance Metrics

### Applications Management
- Paginated List with Sorting
- Advanced Filtering (Date Range, Status, Partner, Customer)
- Search by Process Number or Customer Name
- Inline Editing
- Bulk Operations
- Export to Excel

### System Features
- Role-Based Access Control
- Audit Logging
- Real-time Dashboard Refresh (30-second intervals)
- Responsive Design (Mobile, Tablet, Desktop)
- Dark Theme (Modern Blue/Cyan Accent)

## 🛠️ Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Language** | C# | Latest |
| **.NET** | ASP.NET Core MVC | 9.0 |
| **Database** | SQL Server | 2019+ / Express |
| **ORM** | Entity Framework Core | 9.x |
| **Mapping** | AutoMapper | 12.x |
| **Validation** | FluentValidation | 11.x |
| **Frontend** | Razor Views | - |
| **CSS** | Bootstrap 5 | 5.3 |
| **Charts** | Chart.js | 4.x |
| **Tables** | DataTables | 2.x |
| **Excel** | EPPlus | 8.5.4 |
| **Logging** | Serilog | 4.3.0 |
| **Testing** | xUnit + Moq | Latest |

## 📋 Database Schema

### Tables
- **Applications** - Core transaction table with 10K+ potential records
- **Customers** - Customer master data
- **Partners** - Partner organization data
- **AuditLogs** - Complete audit trail
- **AspNetUsers** - Identity users (ASP.NET Identity)
- **AspNetRoles** - Identity roles

### Key Relationships
```
Partner ────┐
            ├─→ Application
Customer ───┘
```

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server 2019 or SQL Server Express (localhost\SQLEXPRESS01)
- Visual Studio 2022 or VS Code
- Git

### Setup Instructions

1. **Clone Repository**
   ```bash
   git clone <repository-url>
   cd Fintech.Backoffice
   ```

2. **Setup Database**
   - Open SQL Server Management Studio
   - Run `Database\01_CreateDatabase.sql`
   - Run `Database\02_SeedData.sql`

3. **Configure Connection String**
   - Edit `Fintech.Backoffice.Web/appsettings.json`
   - Update `ConnectionStrings.DefaultConnection`

4. **Restore & Build**
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Run Application**
   ```bash
   cd Fintech.Backoffice.Web
   dotnet run
   ```

6. **Access Application**
   - Open: `http://localhost:5000`
   - Default Admin User: (Create via Identity)

## 📖 Project Structure

```
Fintech.Backoffice/
├── Fintech.Backoffice.Domain/          # Entities, Enums, Interfaces
│   ├── Entities/
│   ├── Enums/
│   ├── Interfaces/
│   └── Common/
├── Fintech.Backoffice.Application/      # Services, DTOs, Validators
│   ├── DTOs/
│   ├── ViewModels/
│   ├── Services/
│   ├── Validators/
│   └── Mappings/
├── Fintech.Backoffice.Infrastructure/   # DbContext, Repositories
│   ├── Persistence/
│   └── Services/
├── Fintech.Backoffice.Web/              # ASP.NET Core MVC
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
├── Fintech.Backoffice.Tests/            # Unit Tests
├── Database/                            # SQL Scripts
│   ├── 01_CreateDatabase.sql
│   └── 02_SeedData.sql
├── IMPLEMENTATION_GUIDE.md
└── README.md
```

## 🔑 Key Design Patterns

### Repository Pattern
- Generic `IRepository<T>` interface
- `RepositoryBase<T>` implementation
- Async/await throughout
- LINQ-based queries

### Unit of Work Pattern
- Centralizes repository management
- Ensures atomic transactions
- Single `SaveAsync()` commits all changes
- Rollback on errors

### Service Layer
- Business logic separation
- DTO transformations
- Dashboard calculations
- Data validation

### Dependency Injection
- Constructor injection
- Service registration in `Program.cs`
- Loose coupling between layers

### AutoMapper
- Entity to DTO mapping
- Convention-based configuration
- Custom mappings where needed

## 📊 Database Queries

### Get Dashboard Metrics
```sql
SELECT 
    COUNT(*) as TotalApplications,
    SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) as InProgressCount,
    SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) as CompletedCount,
    SUM(CASE WHEN Status = 3 AND CAST(ApplicationDate AS DATE) = CAST(GETDATE() AS DATE)
            THEN ApprovedAmount ELSE 0 END) as TodayApprovedAmount
FROM Applications;
```

### Get Partner-wise Applications
```sql
SELECT p.PartnerId, p.PartnerName, p.PartnerCode,
       COUNT(a.ApplicationId) as ApplicationCount,
       SUM(CASE WHEN a.Status = 3 THEN 1 ELSE 0 END) as ApprovedCount,
       SUM(CASE WHEN a.Status = 3 THEN a.ApprovedAmount ELSE 0 END) as TotalApproved
FROM Partners p
LEFT JOIN Applications a ON p.PartnerId = a.PartnerId
GROUP BY p.PartnerId, p.PartnerName, p.PartnerCode
ORDER BY ApplicationCount DESC;
```

## 🔐 Authentication & Authorization

### Roles
- **Admin** - Full system access, user management
- **Operations Manager** - Can approve/reject applications
- **Analyst** - View-only access to reports

### Auth Flow
1. User logs in via login page
2. ASP.NET Identity validates credentials
3. JWT/Session token issued
4. Protected routes check authorization
5. Role-based access to features

## 🧪 Testing

Run unit tests:
```bash
dotnet test
```

Test coverage areas:
- Service layer calculations
- Repository LINQ queries
- DTO mappings
- Filter validation

## 📝 Logging

Configured via Serilog:
```
logs/
├── fintech-2026-05-22.txt
├── fintech-2026-05-23.txt
└── ...
```

## 🚢 Deployment

### Local Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
# Deploy to IIS or Azure App Service
```

### Configuration per Environment
- `appsettings.json` - Shared
- `appsettings.Development.json` - Dev
- `appsettings.Production.json` - Prod

## 📚 Documentation

- **IMPLEMENTATION_GUIDE.md** - Step-by-step setup and next steps
- **Code Comments** - Detailed XML comments throughout
- **Architecture Decisions** - Comments explaining design patterns

## 🤝 Contributing

1. Create feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Create Pull Request

## 📞 Support

For questions or issues:
1. Check IMPLEMENTATION_GUIDE.md
2. Review code comments
3. Check database logs
4. Verify connection strings

## 📄 License

This project is proprietary software for FinTech operations.

## 👨‍💻 Author

Generated using Claude AI - Enterprise-grade .NET development

---

**Version:** 1.0 Beta  
**Last Updated:** May 22, 2026  
**Status:** Development Ready
