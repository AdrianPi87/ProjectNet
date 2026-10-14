import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-create-product',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './create-product.html',
  styleUrl: './create-product.css'
})
export class CreateProductComponent {
  private readonly fb = inject(FormBuilder);
  private readonly productService = inject(ProductService);
  private readonly router = inject(Router);

  submitting = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  form = this.fb.group({
    name: ['', Validators.required],
    sku: ['', Validators.required],
    category: ['', Validators.required],
    unitPrice: [0, [Validators.required, Validators.min(0)]]
  });

  submit(): void {
    if (this.form.invalid || this.submitting()) return;
    this.submitting.set(true);
    this.error.set(null);

    const value = this.form.getRawValue();
    this.productService.create({
      name: value.name!,
      sku: value.sku!,
      category: value.category!,
      unitPrice: value.unitPrice!
    }).subscribe({
      next: product => {
        this.submitting.set(false);
        this.success.set(`Product created: ${product.name}`);
        this.form.reset({ unitPrice: 0 });
        this.router.navigate(['/products']);
      },
      error: err => {
        this.submitting.set(false);
        this.error.set(err?.error?.error ?? 'Could not create product.');
      }
    });
  }
}
