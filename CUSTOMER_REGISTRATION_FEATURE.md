# Customer Registration Feature - Implementation Guide

## Overview
This implementation adds a complete customer registration feature that allows staff members to quickly add new customers via a modal dialog from the staff dashboard.

## Frontend Components

### 1. Modal Component (`Modal.tsx`)
- Reusable modal dialog component
- Features:
  - Backdrop with semi-transparent overlay
  - Close button in header
  - Customizable actions in footer
  - Scrollable content area for long forms

### 2. AddCustomerModal Component (`AddCustomerModal.tsx`)
- Modal-specific component for adding customers
- Features:
  - Form with fields: First Name, Last Name, Email, Phone, Address
  - Validation for required fields (First Name, Email, Phone)
  - Error handling and display
  - Loading state during submission
  - Success callback for parent component refresh

### 3. Updated StaffDashboard
- Added state to manage modal visibility
- Connected "Register Customer" quick action button to open modal
- Integrated AddCustomerModal component
- Modal closure handler

### 4. Updated Staff Service (`staffService.ts`)
- Added `AddCustomerRequest` interface
- Added `addCustomer()` method to make POST request to `/staff/customers`

## Backend Implementation

### 1. DTO for Customer Creation (`CreateCustomerDto`)
```csharp
public class CreateCustomerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}
```

### 2. CustomerService Enhancement
- Added `CreateCustomerAsync(CreateCustomerDto dto)` method
- Features:
  - Creates new User with customer role
  - Generates secure temporary password
  - Validates email uniqueness
  - Returns CustomerDto with created customer info

### 3. ICustomerService Interface Update
- Added `CreateCustomerAsync()` method signature

### 4. New Controller: StaffCustomersController
- Route: `/api/staff/customers`
- Authorization: Staff and Admin roles only
- Endpoints:
  - `POST /api/staff/customers` - Create new customer
  - Includes validation and error handling
  - Returns 409 Conflict if email already registered
  - Returns 400 Bad Request for missing required fields

## API Flow

### Request Flow
1. Staff clicks "Register Customer" button on dashboard
2. Modal opens with registration form
3. Staff fills in customer details
4. Form submits to `POST /api/staff/customers`
5. Backend creates User account and assigns customer role
6. Customer receives temporary password
7. Modal closes and dashboard refreshes

### Request Payload Example
```json
{
  "firstName": "Arthur",
  "lastName": "Morgan",
  "email": "arthur@example.com",
  "phone": "+1 (555) 000-0000",
  "address": "123 Main Street"
}
```

### Response Example
```json
{
  "id": 5,
  "fullName": "Arthur Morgan",
  "phone": "+1 (555) 000-0000",
  "email": "arthur@example.com",
  "address": "123 Main Street",
  "registeredDate": "2026-04-30T10:30:00Z",
  "creditBalance": 0,
  "vehicles": [],
  "purchases": []
}
```

## Security Features

1. **Authentication**: Endpoint requires authenticated staff/admin user
2. **Authorization**: Only staff and admin roles can create customers
3. **Password Generation**: Temporary passwords meet complexity requirements
4. **Email Validation**: Prevents duplicate email registrations
5. **Input Validation**: Required fields are validated on both frontend and backend

## User Journey

1. **Staff Dashboard**: Staff member sees "Register Customer" button in Quick Actions
2. **Modal Opens**: Clean, user-friendly form appears
3. **Fill Form**: Required fields are: First Name, Email, Phone
4. **Submit**: After clicking "Add Customer", form validates and submits
5. **Success**: Modal closes, new customer account is created
6. **Result**: Customer can now log in with their credentials

## Error Handling

- **Duplicate Email**: "Email already registered" message displayed
- **Missing Required Fields**: "First Name, Email, and Phone are required" message
- **Server Error**: Generic error message with exception details
- **Network Error**: Graceful error display in modal

## Next Steps (Optional Enhancements)

1. **Customer Verification Email**: Send password reset link instead of temporary password
2. **Bulk Import**: Support importing multiple customers from CSV
3. **Customer Profile**: Show created customer info and provide credentials display
4. **Audit Logging**: Log which staff member created each customer
5. **SMS Notification**: Send customer details via SMS if needed

## Testing Checklist

- [ ] Modal opens when "Register Customer" button clicked
- [ ] Form validation works for required fields
- [ ] Can submit valid customer registration
- [ ] Error message shows for duplicate email
- [ ] New customer can log in with created account
- [ ] Customer has "customer" role assigned
- [ ] Dashboard refreshes after customer creation
- [ ] Modal can be closed via Cancel button
- [ ] Modal can be closed via X button
