-- Script to remove duplicate PascalCase tables and keep lowercase ones
-- Run this script in your PostgreSQL database to clean up duplicates

-- Drop PascalCase tables (keeping lowercase ones)
DROP TABLE IF EXISTS "Appointments" CASCADE;
DROP TABLE IF EXISTS "Invoice" CASCADE;
DROP TABLE IF EXISTS "Invoices" CASCADE;
DROP TABLE IF EXISTS "Notifications" CASCADE;
DROP TABLE IF EXISTS "PartRequests" CASCADE;
DROP TABLE IF EXISTS "Parts" CASCADE;
DROP TABLE IF EXISTS "Reviews" CASCADE;
DROP TABLE IF EXISTS "Saleitems" CASCADE;
DROP TABLE IF EXISTS "Sales" CASCADE;
DROP TABLE IF EXISTS "SalesInvoiceItems" CASCADE;
DROP TABLE IF EXISTS "SalesInvoices" CASCADE;
DROP TABLE IF EXISTS "Users" CASCADE;
DROP TABLE IF EXISTS "Vehicles" CASCADE;

-- Verify remaining tables (should only have lowercase/snake_case)
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
  AND table_type = 'BASE TABLE'
ORDER BY table_name;

-- If you need to recreate any missing tables, the application will auto-migrate them
-- Just run the application after cleaning up duplicates