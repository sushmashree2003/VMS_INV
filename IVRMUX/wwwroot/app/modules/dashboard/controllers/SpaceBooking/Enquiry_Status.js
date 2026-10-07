(function () {
    'use strict';
    angular
        .module('app')
        .controller('Enquiry_Status', Enquiry_Status)
    Enquiry_Status.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Enquiry_Status($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //hide
        $scope.reply = function (enquiry) {            
            $scope.flag = true;
            var data = {
                "OSENQ_Id": enquiry.osenQ_Id,               
            }
            apiService.create("Enquiry_Status/getedit", data).then(function (promise) {               
                $scope.edit_list = promise.edit_list;
                $scope.OSENQ_Id = promise.edit_list[0].osenQ_Id;
            });
        }
        $scope.flag = false;
                
        $scope.submitted = false;
        $scope.load_data = function () {           
            var data = {
                "Type": $scope.OSENQ_Type
            }
            apiService.create("Enquiry_Status/load_data", data).then(function (promise) {               
                $scope.enquirydetailList = promise.enquirydetailList;               
            });
        }
        $scope.submitted = false;
        $scope.ClickBookVisit = function () {           
            var data = {
                "Type": $scope.OSENQ_Type
            }
            apiService.create("Enquiry_Status/ClickBookVisit", data).then(function (promise) {               
                $scope.enquirydetailList = promise.enquirydetailList;
            });
        }
        $scope.submitted = false;
        $scope.SendStatus = function (aa) {
             var a =aa;                       
            var data = {                
                "OSENQ_Id": $scope.OSENQ_Id,
                "Email": $scope.emailid,
                "SMS": $scope.smsid,
                "R_Email": $scope.R_Email,
                "R_SMS": $scope.R_SMS,
                "Body": $scope.bodyid,
                "footer": $scope.footerid
            }
            apiService.create("Enquiry_Status/SendStatus", data).then(function (promise) {               
                $scope.enquirydetailList = promise.datalist;
            });
        }        
    }
})();