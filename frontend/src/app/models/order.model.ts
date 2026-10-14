export interface OrderLine {
  productId: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Order {
  id: string;
  customerId: string;
  status: 'Pending' | 'Confirmed' | 'Shipped' | 'Cancelled';
  total: number;
  createdAtUtc: string;
  lines: OrderLine[];
}

export interface CreateOrderRequest {
  customerId: string;
}

export interface AddOrderLineRequest {
  productId: string;
  quantity: number;
  unitPrice: number;
}
export interface OrdersPage {
  items: Order[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
