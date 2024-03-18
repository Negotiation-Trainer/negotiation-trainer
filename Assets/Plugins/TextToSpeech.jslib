mergeInto(LibraryManager.library, {
    // Function to start speech recognition
    StartSpeechRecognition: function(gameObjectName, callbackName) {
        if ('webkitSpeechRecognition' in window) {
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = false;

            var gameObj = gameObjectName;
            var callbackNM = callbackName;

            // Start recognition
            recognition.onresult = function(event) {
                SendMessage (UTF8ToString(gameObj), UTF8ToString(callbackNM), event.results[event.results.length - 1][0].transcript);
            };

            recognition.start();
        } else {
            console.error('Speech recognition is not supported in this browser.');
        }
    }
});
