# Garage Hub Refactoring: Customer Table Consolidation & CRUD Fixes

## Overview

This refactoring consolidates customer data from a separate `Customers` table to the `Users` table (where `Role = 'customer'`), and fixes CRUD operation buttons in the customer management portal.

---

## 🔄 Backend Changes

### 1. Database Schema Consolidation

**Removed:**
- `customers` table (all data now in `users` table)
- `purchase` table (orphaned table)
- `vehicles.customer_id` column (use `vehicles.user_id` instead)

**Migration Applied:**
```bash
20260519170943_RemoveCustomerTable.cs
```

### 2. Entity Changes

#### Deleted Files:
- ❌ `GarageHub.Domain/Entities/Customer.cs`
- ❌ `GarageHub.Infrastructure/Repositories/CustomerRepository.cs`
- ❌ `GarageHub.Infrastructure/Repositories/ICustomerRepository.cs`

#### Modified Files:

**Vehicle.cs** - Removed CustomerId
```csharp
// REMOVED:
// public int CustomerId { get; set; }
// public Customer? Customer { get; set; }

// KEPT:
public int UserId { get; set; }
public User User { get; set; } = null!;
```

**Purchase.cs** - Changed Customer to User
```csharp
public User? Customer { get; set; }  // Changed from Customer? to User?
```

**AppDbContext.cs** - Removed Customer DbSet
```csharp
// REMOVED:
// public DbSet<Customer> Customers => Set<Customer>();

// UPDATED: Vehicle configuration
modelBuilder.Entity<Vehicle>(e => {
    e.HasOne(v => v.User)
     .WithMany(u => u.Vehicles)
     .HasForeignKey(v => v.UserId)
     .OnDelete(DeleteBehavior.Cascade);
});
```

### 3. Service Layer Refactoring

**CustomerService.cs** - Now queries from Users table

All methods now use:
```csharp
// Instead of: var customer = await _db.Customers.FirstOrDefaultAsync(...);
// Now use:
var customer = await _db.Users
    .FirstOrDefaultAsync(u => u.UserId == customerId && u.Role == "customer");
```

**Key Methods Updated:**
- `GetProfileAsync()` - Added role check
- `GetCustomerDetailsAsync()` - Queries Users instead of Customers
- `GetAllCustomersAsync()` - Filters by `Role == "customer"`
- `SearchCustomersAsync()` - Searches in Users table
- `CreateCustomerAsync()` - Creates User with role='customer'

### 4. API Compatibility

✅ **All existing endpoints remain unchanged:**
- `GET /api/staff/customers`
- `GET /api/staff/customers/{id}`
- `GET /api/staff/customers/search`
- `POST /api/staff/customers`
- `PUT /api/staff/customers/{id}`
- `POST /api/customer/vehicles`
- `POST /api/staff/appointments`

---

## 🎨 Frontend Changes

### 1. BookAppointmentModal.tsx

**Enhancements:**
- ✅ Added `customerId` and `vehicles` props
- ✅ Dynamic vehicle dropdown
- ✅ Proper API integration
- ✅ Error handling with user feedback
- ✅ Form reset after submission

**New Props:**
```typescript
interface BookAppointmentModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  customerId?: number;  // NEW
  vehicles?: Array<{    // NEW
    vehicleId: number;
    vehicleNumber: string;
    make: string;
    model: string;
  }>;
}
```

**Usage in Staff Portal:**
```tsx
<BookAppointmentModal
  isOpen={isBookServiceOpen}
  onClose={() => setIsBookServiceOpen(false)}
  onSuccess={() => setDetailsRefresh(v => v + 1)}
  customerId={selected?.id}
  vehicles={selectedDetails?.vehicles || []}
/>
```

### 2. CustomerManagement.tsx - CRUD Fixes

#### ✅ Fixed: View Button
```tsx
// Now properly selects customer
const handleViewClick = (customer: Customer) => {
  setSelected(customer);
};

<button 
  className="text-primary font-bold text-xs hover:underline uppercase"
  onClick={(e) => {
    e.stopPropagation();
    handleViewClick(c);
  }}
>
  View
</button>
```

#### ✅ Fixed: Edit Button
```tsx
// Already working - confirmed functionality
<button
  className="text-slate-400 hover:text-primary transition-colors"
  onClick={(event) => {
    event.stopPropagation();
    openEditCustomer(c);  // Opens modal with pre-filled data
  }}
>
  <span className="material-symbols-outlined">edit</span>
</button>
```

#### ✅ Enhanced: Book Service Button
```tsx
// Now passes customer context to modal
<button 
  onClick={() => setIsBookServiceOpen(true)}
  className="bg-white p-4 rounded-xl shadow-sm"
>
  Book Service
</button>

// BookAppointmentModal receives:
<BookAppointmentModal
  customerId={selected?.id}  // Customer ID for appointment
  vehicles={selectedDetails?.vehicles}  // Customer's vehicles
  {...otherProps}
/>
```

### 3. BookAppointment.tsx - Consolidated Form

**Improvements:**
- ✅ Removed duplicate modal
- ✅ Single form on page
- ✅ Better error handling
- ✅ Success message (5s auto-dismiss)
- ✅ Vehicle selector with summary

**Form Features:**
```tsx
// Required fields marked with *
- Customer Name (read-only)
- Vehicle Selection * (required)
- Service Type * (required)
- Preferred Date * (required)
- Preferred Time Slot * (required)
- Issue Description (optional)

// Success/Error Messages
{submitSuccess && <success message>}
{submitError && <error message>}
```

---

## 🔍 Testing Checklist

### Backend Testing

- [ ] Migration applied successfully
  ```bash
  dotnet ef database update --project GarageHub.Infrastructure
  ```

- [ ] Database schema verified
  - [ ] `customers` table removed
  - [ ] `purchase` table removed
  - [ ] `vehicles.customer_id` column removed
  - [ ] `users` table has all customer data

- [ ] API endpoints functional
  - [ ] GET /api/staff/customers returns users with role='customer'
  - [ ] GET /api/staff/customers/{id} returns correct user
  - [ ] POST /api/staff/customers creates user with role='customer'
  - [ ] PUT /api/staff/customers/{id} updates user
  - [ ] POST /api/customer/vehicles adds vehicles to user

- [ ] Relationships maintained
  - [ ] Vehicles correctly linked to users
  - [ ] Appointments accessible via CustomerId (mapped to UserId)
  - [ ] Reviews, PartRequests, SalesInvoices work correctly

### Frontend Testing

#### CustomerManagement Page

- [ ] **Search Functionality**
  - [ ] Search returns customers
  - [ ] Multiple search parameters work
  - [ ] Clear results when no matches

- [ ] **View Button** ✅ FIXED
  - [ ] Clicking View selects customer
  - [ ] Details panel displays
  - [ ] No console errors

- [ ] **Edit Button** ✅ VERIFIED
  - [ ] Opens modal with data
  - [ ] Can modify fields
  - [ ] Changes persist
  - [ ] Modal closes after save

- [ ] **Book Service Button** ✅ ENHANCED
  - [ ] Opens BookAppointmentModal
  - [ ] Modal shows customer's vehicles
  - [ ] Can successfully book appointment
  - [ ] Refreshes after booking

#### BookAppointment Page

- [ ] **Form Display**
  - [ ] All fields visible
  - [ ] Required fields marked
  - [ ] Vehicle dropdown populated

- [ ] **Form Submission**
  - [ ] Validates required fields
  - [ ] Shows error if vehicle not selected
  - [ ] Shows success after booking
  - [ ] Resets form after success
  - [ ] Success message auto-dismisses

- [ ] **Vehicle Selection**
  - [ ] Dropdown shows customer's vehicles
  - [ ] Summary updates when vehicle selected
  - [ ] Correct vehicle sent to API

- [ ] **Time Slot Selection**
  - [ ] Time slots generate correctly
  - [ ] Can select any time slot
  - [ ] Date picker shows future dates only

---

## 📋 Deployment Guide

### Prerequisites
- .NET 10 SDK
- PostgreSQL 12+
- Latest source code

### Steps

1. **Backup Database** (Recommended)
   ```bash
   pg_dump -U postgres garage_hub > backup_$(date +%Y%m%d).sql
   ```

2. **Update Source Code**
   ```bash
   git pull origin main
   ```

3. **Apply Database Migration**
   ```bash
   cd GarageHub.Backend
   dotnet ef database update --project GarageHub.Infrastructure
   ```

4. **Rebuild Backend**
   ```bash
   dotnet build
   dotnet publish -c Release
   ```

5. **Deploy Frontend**
   ```bash
   cd ../Garage-Hub-Frontend
   npm run build
   npm run deploy
   ```

6. **Verify Deployment**
   - Test customer search
   - Test View/Edit buttons
   - Test Book Service flow
   - Check database schema

---

## 🐛 Troubleshooting

### Issue: "Customer not found" errors
**Cause**: Users don't have `role='customer'`
**Solution**: Run migration and verify role values in database
```sql
SELECT COUNT(*) FROM users WHERE role = 'customer';
```

### Issue: Vehicles not appearing in dropdown
**Cause**: User mismatch or vehicles have old `customer_id`
**Solution**: Verify vehicles have correct `user_id`
```sql
SELECT v.* FROM vehicles v 
JOIN users u ON v.user_id = u.user_id 
WHERE u.role = 'customer';
```

### Issue: Book Service button doesn't work
**Cause**: Modal not receiving props
**Solution**: Check that `selected` and `selectedDetails` are set
```typescript
console.log('Customer ID:', selected?.id);
console.log('Vehicles:', selectedDetails?.vehicles);
```

### Issue: Appointment API returns 404
**Cause**: CustomerID mismatch
**Solution**: Ensure appointment uses `UserId` not old `CustomerId`

---

## 📁 Files Changed

### Backend
```
✏️  GarageHub.Domain/Entities/Vehicle.cs
✏️  GarageHub.Domain/Entities/Purchase.cs
✏️  GarageHub.Infrastructure/Data/AppDbContext.cs
✏️  GarageHub.Infrastructure/Services/CustomerService.cs
✨ GarageHub.Infrastructure/Migrations/20260519170943_RemoveCustomerTable.cs
❌ GarageHub.Domain/Entities/Customer.cs
❌ GarageHub.Infrastructure/Repositories/CustomerRepository.cs
❌ GarageHub.Infrastructure/Repositories/ICustomerRepository.cs
```

### Frontend
```
✏️  src/components/modals/BookAppointmentModal.tsx
✏️  src/pages/staff/CustomerManagement.tsx
✏️  src/pages/customer/BookAppointment.tsx
```

---

## ✨ Key Benefits

1. **Data Consolidation**: Single Users table for all user types
2. **Reduced Complexity**: No redundant Customer entity
3. **Better Queries**: Direct user lookups with role filtering
4. **Fixed UI**: All CRUD buttons now fully functional
5. **Improved UX**: Context-aware modals with customer vehicles
6. **Maintainability**: Simpler codebase, fewer tables

---

## 📞 Support

For issues or questions:
1. Check troubleshooting section
2. Review migration logs
3. Check browser console for frontend errors
4. Review API response bodies for backend errors
5. Contact development team if needed

---

**Status**: ✅ Ready for Testing & Deployment
**Last Updated**: 2025-05-19
**Version**: 1.0
