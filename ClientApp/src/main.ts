import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule) // Uygulama çalıştığı zaman AppModule çalışacak. AppModule içerisinde de AppComponent çalışacak.
// bootstrap burada ilk çalışacak anlamına gelir
  .catch(err => console.error(err));
