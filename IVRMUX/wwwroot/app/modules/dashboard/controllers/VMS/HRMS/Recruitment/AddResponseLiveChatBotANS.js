(function () {
    'use strict';
    angular
        .module('app')
        .controller('AddResponseLiveChatBotANSController', AddResponseLiveChatBotANSController)
    AddResponseLiveChatBotANSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function AddResponseLiveChatBotANSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.newResponse = {
            CTFAQMQ_FAQ: '',
            CTFAQMQ_Remarks: '',
            CTFAQMQANS_Answers: '',
            CTFAQMQANS_Flg: 'text',
            Keywords: []
        };


        $scope.searchValue = "";
        $scope.faqs = [];
        $scope.successMessage = '';
        $scope.errorMessage = '';
        $scope.isLoading = false;
        $scope.firstClick = true;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 10;
        $scope.loadFAQs = function () {
            $scope.isLoading = true;

            var pageid = 1;

            apiService.getURI("AddResponseLiveChatBot/GetAllFAQs", pageid).then(function (promise) {

                $scope.faqs = promise.qetanswer;
                console.log($scope.faqs.Keywords);

            })
                .catch(function (error) {
                    $scope.errorMessage = 'Failed to load FAQs!';
                })
                .finally(() => {
                    $scope.isLoading = false;
                });
        };


        $scope.loadFAQs();

        //$scope.addKeyword = function () {
        //    $scope.newResponse.Keywords.push({ CTFAQMQKW_Keywords: '' });

        //    if ($scope.firstClick) {
        //        alert('First Keyword section: You can add save what you written in Question.');
        //        $scope.firstClick = false;
        //    }
        //};

        $scope.removeKeyword = function (index) {
            $scope.newResponse.Keywords.splice(index, 1);
        };


        $scope.addResponse = function () {
            if (!$scope.newResponse.CTFAQMQ_FAQ.trim() ||
                !$scope.clientnames_rem.length ||
                $scope.clientnames_rem.some(keyword => !keyword.CTFAQMQKW_Keywords.trim()) ||
                !$scope.newResponse.CTFAQMQANS_Answers.trim()) {
                swal('FAQ, Keywords, and Answer cannot be empty!');
                return;
            }

            $scope.isLoading = true;
            $scope.successMessage = '';
            $scope.errorMessage = '';

            var data = {
                "CTFAQMQ_FAQ": $scope.newResponse.CTFAQMQ_FAQ,
                "CTFAQMQ_Remarks": $scope.newResponse.CTFAQMQ_Remarks,
                //Keywords: $scope.newResponse.Keywords.map(keyword => ({
                //    "CTFAQMQKW_Keywords": keyword.CTFAQMQKW_Keywords,
                //})),
                Keywords: $scope.clientnames_rem.map(keyword => ({
                    "CTFAQMQKW_Keywords": keyword.CTFAQMQKW_Keywords
                })),
                answers: [{
                    "CTFAQMQANS_Answers": $scope.newResponse.CTFAQMQANS_Answers,
                }]
            };

            apiService.create("AddResponseLiveChatBot/savedata", data)
                .then(function (promise) {

                    if (promise.returndata === "Add") {
                        swal('Record Saved Successfully !');

                    }
                    else if (promise.returndata === "Notsave") {
                        swal("Record Not Saved !");

                    }
                    else if (promise.returndata === "Duplicate") {
                        swal("Record Already Exists !");

                    }
                    else if (promise.returndata == "Update") {
                        swal("Record Update Successfully");
                    }
                    else {
                        swal("Please Contact Administrator !");
                    }
                    $state.reload();
                });

        };


        //added by Nagaraj
        //$scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12', CTFAQMQKW_Keywords: '' }];
        $scope.addgrnrowsclient = function () {
            swal('First Keyword section: You can add save what you written in Question.');
            if ($scope.clientnames_rem.length > 1) {

                for (var x = 0; x === $scope.clientnames_rem.length; x++) {
                    var id1 = $scope.clientnames_rem[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.clientnames_rem.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };

        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.clientnames_rem.length - 1;
            $scope.clientnames_rem.splice(index, 1);

        };

        $scope.interacted = function (field) {
            return field.$touched || $scope.submitted || field.$dirty;
        };

        $scope.itemsPerPages = paginationformasters;
        $scope.currentPages = 1;
        $scope.PreviousresultData = [];


        $(document).ready(function () {
            // Initialize Kendo Editor
            $("#editor").kendoEditor({
                tools: [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "createLink",
                    "unlink",
                    "insertImage",
                    "insertFile",
                    "subscript",
                    "superscript",
                    "tableWizard",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "mergeCellsHorizontally",
                    "mergeCellsVertically",
                    "splitCellHorizontally",
                    "splitCellVertically",
                    "formatting",
                    "cleanFormatting",
                    "copyFormat",
                    "applyFormat",
                    "fontName",
                    "fontSize",
                    "foreColor",
                    "backColor",
                    "print",
                    "viewHtml"
                ],
                change: function () {
                    // Update ng-model whenever the content changes
                    var value = this.value();
                    angular.element($('#editor')).scope().newResponse.CTFAQMQANS_Answers = value;
                    angular.element($('#editor')).scope().$apply();
                }
            });
        });



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



