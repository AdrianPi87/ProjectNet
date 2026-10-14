export interface Customer {
  id: string;
  name: string;
  email: string;
  createdAtUtc: string;
}

export interface CustomerDto extends Customer {}

export interface Product {
  id: string;
  name: string;
  sku: string;
  category: string;
  unitPrice: number;
}
