mergeInto(LibraryManager.library, {
    PostJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
        
        var newPostKey = firebase.database().ref().child(parsedPath).push().key;

        var postData = {
            value: parsedValue
        };

        // Write the new post's data simultaneously in the posts list and the user's post list.
        var updates = {};
        updates['/' + parsedPath + '/' + newPostKey] = postData;

        firebase.database().ref().update(updates).then((unityInstance) => {
            TestAlert("Post Success");
            //fullscreenButton.onclick = () => { unityInstance.SetFullscreen(1); };
        }).catch((message) => {
            TestAlert("Post Fail");
        });;

    },
    UploadMap: function(name, value, objectName, callback, fallback) {
        var parsedName = Pointer_stringify(name);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
        
        var batch = db.batch();

        // Set the value of 'NYC'
        var mapUnit = db.collection("MapData").doc();
        var mapId = db.collection("MapId").doc();

        batch.set(mapId, {
          "key": mapUnit.id,
          "name": parsedName
        });
        TestAlert(parsedValue);
        batch.set(mapUnit, JSON.parse(parsedValue));

        batch.commit();

    },
    GetLevelData: function(path, objectName, callback) {
        
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);

        var docRef = db.collection("MapData").doc(parsedPath);

        docRef.get().then((doc) => {
            if (doc.exists) {
                TestAlert(JSON.stringify(doc.data()));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(doc.data()));
            
            } else {
                TestAlert("Fail");
            }
        }).catch((error) => {
            TestAlert("TTT: Fail");
        });


    },
    GetJSON: function(objectName, callback, fallback) {
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
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
            TestAlert(JSON.stringify(retval));
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(retval));
        });
    },
    WebAlert: function(value) {
        TestAlert(Pointer_stringify(value));

    }
    /*
    GetJSON: function(objectName, callback, fallback) {
        
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);

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
            TestAlert(JSON.stringify(retval));
            
        }).catch(error => {
            console.error('Failed to fetch documents:', error);
        });

    },
    GetJSON: function(path, objectName, callback, fallback) {
        
        var parsedPath = Pointer_stringify(path);
        parsedPath = "Leader";
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);

        firebase.database().ref(parsedPath).get().then((snapshot) => {
            if (snapshot.exists()) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot));
                TestAlert(JSON.stringify(snapshot));
            } else {
                console.log("No data available");
            }
        }).catch((error) => {
            console.error(error);
        });
        
        firebase.database().ref(parsedPath).on('value', (snapshot) => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot));
            TestAlert(JSON.stringify(snapshot));
        });


    },
    */
 });