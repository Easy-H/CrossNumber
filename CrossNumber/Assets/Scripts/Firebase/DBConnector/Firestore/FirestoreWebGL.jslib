mergeInto(LibraryManager.library, {
    FirestoreAddRecord: function(pathJson, updateJson) {

        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }

        var updates = JSON.parse(UTF8ToString(updateJson));
        for (var key in updates) {
            if (updates.hasOwnProperty(key)) {
                if (updates[key] === null) {
                    updates[key] = firebase.firestore.FieldValue.delete();
                }
            }
        }
        
        docRef.set(updates).then(() => {
                console.log("Document successfully added!");
            }).catch((error) => {
                console.error("Error writing document: ", error);
            });

    },
    FirestoreUpdateRecord: function(pathJson, updateJson){
        
        var parsedPath = JSON.parse(UTF8ToString(pathJson));

        var docRef = firebase.firestore().collection(parsedPath[0]).doc(parsedPath[1]);

        for (var i = 2; i < parsedPath.length; i += 2) {
            docRef = docRef.collection(parsedPath[i]).doc(parsedPath[i + 1]);
        }

        var updates = JSON.parse(UTF8ToString(updateJson));
        for (var key in updates) {
            if (updates.hasOwnProperty(key)) {
                if (updates[key] === null) {
                    updates[key] = firebase.firestore.FieldValue.delete();
                }
            }
        }
        
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
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(doc.data()));
            } else {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, "Network Error");
            }
        }).catch((error) => {
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