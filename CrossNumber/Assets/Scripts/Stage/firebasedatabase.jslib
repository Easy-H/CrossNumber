mergeInto(LibraryManager.library, {
    FirestoreConnect: function(path, firebaseConfigValue) {
        
        // TODO: Add SDKs for Firebase products that you want to use
        // https://firebase.google.com/docs/web/setup#available-libraries
        
        // Your web app's Firebase configuration
        // For Firebase JS SDK v7.20.0 and later, measurementId is optional

        var firebaseConfig = JSON.parse(UTF8ToString(firebaseConfigValue));
        
        firebaseApp = firebase.initializeApp(firebaseConfig);
        
        auth = firebaseApp.auth();
        db = firebase.firestore();

    },
    PostJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        
        var newPostKey = firebase.database().ref().child(parsedPath).push().key;

        var postData = {
            value: parsedValue
        };

        // Write the new post's data simultaneously in the posts list and the user's post list.
        var updates = {};
        updates['/' + parsedPath + '/' + newPostKey] = postData;

        firebase.database().ref().update(updates).then((unityInstance) => {
            //fullscreenButton.onclick = () => { unityInstance.SetFullscreen(1); };
        }).catch((message) => {
        });;

    },
    UploadMap: function(name, value, objectName, callback, fallback) {
        var parsedName = UTF8ToString(name);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        
        var batch = db.batch();

        // Set the value of 'NYC'
        var mapUnit = db.collection("MapData").doc();
        var mapId = db.collection("MapId").doc();

        batch.set(mapId, {
          "key": mapUnit.id,
          "name": parsedName
        });
        batch.set(mapUnit, JSON.parse(parsedValue));

        batch.commit();

        console.log(parsedValue);

    },
    GetLevelData: function(path, objectName, callback) {
        
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);

        var docRef = db.collection("MapData").doc(parsedPath);

        docRef.get().then((doc) => {
            if (doc.exists) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(doc.data()));
            
            } else {
            }
        }).catch((error) => {
        });


    },
    GetJSON: function(objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        db.collection("MapId").get().then((snapshot) => {
            var documents = [];
            snapshot.forEach((documentSnapshot) => {
                documents.push(documentSnapshot);
            });
            var documentsToFetch = Math.min(10, documents.length);
            var randomDocuments = [];
            for (let i = 0; i < documentsToFetch; i++) {
                var randomIndex = Math.floor(Math.random() * documents.length);
                randomDocuments.push(documents[randomIndex]);
                documents.splice(randomIndex, 1);
            }
            var retval = {};
            randomDocuments.forEach((ds) => {
                retval[ds.id] = ds.data();
            });
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(retval));
        });
    },
    WebAlert: function(value) {

    }
    /*
    GetJSON: function(objectName, callback, fallback) {
        
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);

        var collectionRef = db.collection("MapId");

        collectionRef.get().then(snapshot => {
            // 가져온 문서 목록
            var documents = [];
        
            snapshot.forEach(documentSnapshot => {
                documents.push(documentSnapshot);
            });
        
            // 무작위로 X개의 문서 선택
            var documentsToFetch = Math.min(10, documents.length);
            var randomDocuments = [];
        
            for (let i = 0; i < documentsToFetch; i++) {
                var randomIndex = Math.floor(Math.random() * documents.length);
                randomDocuments.push(documents[randomIndex]);
                documents.splice(randomIndex, 1);
            }
        
            var retval = {};
        
            randomDocuments.forEach(ds => {
                retval[ds.id] = ds.data();
            });
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(retval));
            
        }).catch(error => {
            console.error('Failed to fetch documents:', error);
        });

    },
    GetJSON: function(path, objectName, callback, fallback) {
        
        var parsedPath = UTF8ToString(path);
        parsedPath = "Leader";
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        firebase.database().ref(parsedPath).get().then((snapshot) => {
            if (snapshot.exists()) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot));
            } else {
                console.log("No data available");
            }
        }).catch((error) => {
            console.error(error);
        });
        
        firebase.database().ref(parsedPath).on('value', (snapshot) => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot));
        });


    },
    */
 });