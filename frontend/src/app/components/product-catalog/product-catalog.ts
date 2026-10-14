import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-catalog',
  standalone: true,
  imports: [RouterLink, DecimalPipe, FormsModule],
  templateUrl: './product-catalog.html',
  styleUrl: './product-catalog.css'
})
export class ProductCatalogComponent implements OnInit {
  private readonly productService = inject(ProductService);

  products = signal<Product[]>([]);
  categories = signal<string[]>([]);
  searchTerm = signal('');
  selectedCategory = signal('ALL');

  filteredProducts = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();
    const category = this.selectedCategory();
    return this.products().filter(product => {
      const matchesCategory = category === 'ALL' || product.category === category;
      const matchesTerm = !term
        || product.name.toLowerCase().includes(term)
        || product.sku.toLowerCase().includes(term)
        || product.category.toLowerCase().includes(term);
      return matchesCategory && matchesTerm;
    });
  });

  ngOnInit(): void {
    this.productService.getAll().subscribe(products => {
      this.products.set(products);
      this.categories.set(['ALL', ...new Set(products.map(product => product.category))].sort());
    });
  }

  onSearch(value: string): void { this.searchTerm.set(value); }
  onCategory(value: string): void { this.selectedCategory.set(value); }
}


