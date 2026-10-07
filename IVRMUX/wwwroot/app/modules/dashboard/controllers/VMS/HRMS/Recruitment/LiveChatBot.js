(function () {
    'use strict';

    angular.module('app', [])
        .controller('LiveChatBotController', LiveChatBotController)
        .service('apiService', apiService);

    LiveChatBotController.$inject = ['$scope', 'apiService', '$sce'];

    function LiveChatBotController($scope, apiService, $sce) {
        $scope.messages = [];
        $scope.userMessage = '';
        $scope.isLoading = false;

        $scope.sendMessage = function () {
            if ($scope.userMessage.trim() === '') return;

            $scope.isLoading = true;

            $scope.messages.push({
                sender: 'client',
                text: $scope.userMessage
            });

            var data = {
                "userMessage": $scope.userMessage
            };

            apiService.create("AddResponseLiveChatBot/GetBotResponse", data).then(function (response) {
                //$scope.keywordSuggestions = [];
                $scope.keywordSuggestions = response.KeywordSuggestions;

                if (response.qetanswer != null) {

                    var answer = response.qetanswer[0].ctfaqmqanS_Answers;
                    $scope.messages.push({
                        sender: 'bot',
                        text: answer
                    });


                    if (response.Keywords && response.Keywords.length > 0) {
                        $scope.keywordSuggestions = response.Keywords.map(k => k.Keyword);
                    }
                } else {
                    $scope.messages.push({
                        sender: 'bot',
                        text: "I'm sorry, I don't have an answer for that."
                    });
                }
            }).catch(function (error) {
                console.error('Error occurred:', error);
                $scope.messages.push({
                    sender: 'bot',
                    text: 'An error occurred while fetching the response!'
                });
            }).finally(() => {

                $scope.userMessage = '';
                $scope.isLoading = false;
            });
        };

        $scope.checkKeyPress = function (event) {
            if (event.which === 13) {
                $scope.sendMessage();
            }
        };
    }

    apiService.$inject = ['$http'];

    function apiService($http) {
        this.create = function (url, data) {
            return $http.post(url, data, {
                headers: { 'Content-Type': 'application/json' }
            });
        };
    }
})();
