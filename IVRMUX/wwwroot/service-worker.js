
//var cacheName = 'v1.0';

//var cacheAssets = [
//    'index.html'    
//]

//// installation
//self.addEventListener('install', e => {
//    console.log('Service Worker: Installed');
//    e.waitUntil(
//        caches
//            .open(cacheName)
//            .then(cache => {
//                console.log('Service Worker: Caching Files');
//                cache.addAll(cacheAssets);
//            })
//            .then(() => self.skipWaiting())
//    );

//});

//// activation
//self.addEventListener('activate', e => {
//    console.log('Service Worker: Installed');
//});

//// fetch

//self.addEventListener('fetch', e => {
//    $.getJSON('https://ipinfo.io/json?token=a28739c42d2f25', function (data) {       
//        console.log(data.ip);
//    });
//    console.log('Service Worker: Fetching');
//    e.respondWith(fetch(e.request).catch(() => caches.match(e.request)));

//});