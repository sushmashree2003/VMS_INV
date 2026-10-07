// Import and configure the Firebase SDK
// These scripts are made available when the app is served or deployed on Firebase Hosting
// If you do not serve/host your project using Firebase Hosting see https://firebase.google.com/docs/web/setup
importScripts('https://www.gstatic.com/firebasejs/8.6.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.6.1/firebase-messaging.js');

firebase.initializeApp({
    'apiKey': 'AIzaSyC7RLFr8bkITn06hSWoDaDoLt5AZp_kHb8',
    'authDomain': 'vapsstofware.firebaseapp.com',
    'projectId': 'vapsstofware',
    'storageBucket': 'vapsstofware.appspot.com',
    'appId': '1:63236243897:web:200456b0df7c928670b668',
    'measurementId': "G-TWY1L7JMVW",
    'messagingSenderId': '63236243897'
});
const messaging = firebase.messaging();
/**
 * Here is is the code snippet to initialize Firebase Messaging in the Service
 * Worker when your app is not hosted on Firebase Hosting.

 // [START initialize_firebase_in_sw]
 // Give the service worker access to Firebase Messaging.
 // Note that you can only use Firebase Messaging here, other Firebase libraries
 // are not available in the service worker. 

 // Initialize the Firebase app in the service worker by passing in the
 // messagingSenderId. 

 // Retrieve an instance of Firebase Messaging so that it can handle background
 // messages.
 const messaging = firebase.messaging();
 // [END initialize_firebase_in_sw]
 **/
// If you would like to customize notifications that are received in the
// background (Web app is closed or not in browser focus) then you should
// implement this optional method.
// [START background_handler]
messaging.onBackgroundMessage((payload) => {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    var notificationTitle = payload.notification.title; //or payload.notification or whatever your payload is
    var notificationURL = payload.data.key1;
    var notificationicon = payload.data.key2;
    var notificationOptions = {
        body: payload.notification.body,
        icon: notificationicon,
        data: { url: notificationURL },        
        actions: [{ action: "open_url", title: "View Details" }]
    };
   return self.registration.showNotification(notificationTitle,
        notificationOptions);
});
self.addEventListener('notificationclick', function (event) {
    switch (event.action) {
        case 'open_url':
            clients.openWindow(event.notification.data.url); //which we got from above
            break;
        case 'any_other_action':
            clients.openWindow("http://localhost:57606/#/app/ISM_ComplaintsTaskList/729");
            break;
    }
}, false);
// [END background_handler]
