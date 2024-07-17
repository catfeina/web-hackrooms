import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlatformService {

  constructor() { }

  isBrowser(): boolean {
    return typeof window !== 'undefined';
  }
}
