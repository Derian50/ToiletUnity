mergeInto(LibraryManager.library, {
	ShowAd: function () {
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onOpen: function() {
					myGameInstance.SendMessage('YaSDK', 'OnOpen');
				},
				onClose: function(wasShown) {
					myGameInstance.SendMessage('YaSDK', 'OnClose');
				},
				onError: function(error) {
					myGameInstance.SendMessage('YaSDK', 'OnError');
				},
				onOffline: function(error) {
					myGameInstance.SendMessage('YaSDK', 'OnOffline');
				}
			}
		})
	},
	ShowReward: function() {
		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					myGameInstance.SendMessage('YaSDK', 'OnOpenReward');
				},
				onRewarded: () => {
					myGameInstance.SendMessage('YaSDK', 'OnRewarded');
				},
				onClose: () => {
					myGameInstance.SendMessage('YaSDK', 'OnCloseReward');
				}, 
				onError: (e) => {
					myGameInstance.SendMessage('YaSDK', 'OnErrorReward');
				}
			}
		})
	},
	GetPlayerData: function(){
		player.getData().then(dataJson => {
			var dataString = JSON.stringify(dataJson);
			myGameInstance.SendMessage('YaSDK', 'OnGetData', dataString);
		});
	},
	SetPlayerData: function(dataObj){
		var dataString = UTF8ToString(dataObj);
		var dataJson = JSON.parse(dataString);
		player.setData(dataJson);
	},
	GetLang : function (){
		var lang = ysdk.environment.i18n.lang;
		var bufferSize = lengthBytesUTF8(lang) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(lang, buffer, bufferSize);
		return buffer;
	},
	SaveExtern: function(date) {
    	var dateString = UTF8ToString(date);
    	var myobj = JSON.parse(dateString);
    	player.setData(myobj);
  	},

  	LoadExtern: function(){
    	player.getData().then(_date => {
        	const myJSON = JSON.stringify(_date);
        	myGameInstance.SendMessage('Progress', 'SetPlayerInfo', myJSON);
    	});
 	}
});
