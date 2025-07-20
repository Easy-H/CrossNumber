mergeInto(LibraryManager.library, {
    FirestoreConnect: function(firebaseConfigValue) {
        
        // TODO: Add SDKs for Firebase products that you want to use
        // https://firebase.google.com/docs/web/setup#available-libraries
        
        // Your web app's Firebase configuration
        // For Firebase JS SDK v7.20.0 and later, measurementId is optional

        var firebaseConfig = JSON.parse(UTF8ToString(firebaseConfigValue));
        
        firebaseApp = firebase.initializeApp(firebaseConfig);

    },
    FirestoreAddRecord: function(pathJson, recordJson, idx) {

        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }
        
        var parsedIdx = JSON.parse(UTF8ToString(idx));
        
        var up = {};
        up[parsedIdx] = JSON.parse(UTF8ToString(recordJson));

        docRef.set(up)
        .then(() => {
            console.log("Document successfully written!");
        })
        .catch((error) => {
            console.error("Error writing document: ", error);
        });

    },
    FirestoreUpdateRecordAt: function(pathJson, recordJson, idx){
        
        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }

        var parsedIdx = JSON.parse(UTF8ToString(idx));

        var updates = {};
        updates[parsedIdx] = JSON.parse(UTF8ToString(recordJson));

        docRef.update(updates)
        .then(() => {
            console.log("Document successfully updated!");
        })
        .catch((error) => {
            console.error("Error writing document: ", error);
        });

    },
    FirestoreGetAllRecord: function(pathJson, objectName, callback, fallback) {
        
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        
        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }
        
        docRef.get().then((doc) => {
            if (doc.exists) {
                console.log("Document data:", doc.data());
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(doc.data()));
            } else {
                console.log("No such document!");
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, "No Idx");
            }
        }).catch((error) => {
            console.log("Error getting document:", error);
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, "Network Error");
        });

    },
    FirestoreDeleteRecordAt: function(pathJson, idx) {

        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }

        var parsedIdx = JSON.parse(UTF8ToString(idx));

        var updates = {};
        updates[parsedIdx] = firebase.firestore.FieldValue.delete();

        docRef.update(updates)
        .then(() => {
            console.log("Document successfully updated!");
        })
        .catch((error) => {
            console.error("Error writing document: ", error);
        });

    }
 });