mergeInto(LibraryManager.library, {
    IsConnect: function () {

        const user = firebase.auth().currentUser;

        if (user) {
            return true;
        } else {
            return false;
        }

    },
    FirebaseAuthCurrentUser: function () {
        returnStr = JSON.stringify(firebase.auth().currentUser)
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    FirebaseAuthSignOut: function (objectName, callback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);

        firebase.auth().signOut().then(() => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback);

        })
    },
    FirebaseAuthSignIn: function (id, pw, objectName, callback, fallback) {

        var parsedId = UTF8ToString(id);
        var parsedPw = UTF8ToString(pw);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);


        firebase.auth().signInWithEmailAndPassword(parsedId, parsedPw)
            .then((userCredential) => {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(userCredential.user));
            })
            .catch((error) => {
                var errorMessage = error.message;
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);
            });
    },
    FirebaseAuthSignUp: function (id, pw, objectName, callback, fallback) {

        var parsedId = UTF8ToString(id);
        var parsedPw = UTF8ToString(pw);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        firebase.auth().createUserWithEmailAndPassword(parsedId, parsedPw)
            .then((userCredential) => {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback);
            })
            .catch((error) => {
                var errorMessage = error.message;
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);
            });
    },
    FirebaseAuthDelete: function (objectName, callback, fallback) {

        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        const user = firebase.auth().currentUser;

        user.delete().then(() => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback);

        }).catch((error) => {
            var errorMessage = error.message;
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);

        });
    },
    FirebaseAuthUpdatePW: function (pw, objectName, callback, fallback) {

        var parsedPW = UTF8ToString(pw);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        const user = firebase.auth().currentUser;

        user.updatePassword(parsedPW).then(() => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback);

        }).catch((error) => {
            var errorMessage = error.message;
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);

        });

    },
    FirebaseAuthUpdateName: function (name, objectName, callback, fallback) {

        var parsedName = UTF8ToString(name);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        const user = firebase.auth().currentUser;

        user.updateProfile({
            displayName: parsedName
        }).then(() => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback);

        }).catch((error) => {
            var errorMessage = error.message;
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);

        })
    },
    FirebaseAuthReverify: function (pw, objectName, callback, fallback) {

        var parsedPW = UTF8ToString(pw);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        const user = firebase.auth().currentUser;

        // TODO(you): prompt the user to re-provide their sign-in credentials
        const credential = firebase.auth.EmailAuthProvider.credential(
            user.email,
            parsedPW
        );
        
        user.reauthenticateWithCredential(credential).then(() => {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback);
        }).catch((error) => {
            var errorMessage = error.message;
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, errorMessage);
        });

    }
});