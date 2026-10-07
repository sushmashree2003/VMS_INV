(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeFandFReportController', EmployeeFandFReportController)

    EmployeeFandFReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function EmployeeFandFReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("CumulativeSalaryReport/getOnloaddata", id).
                then(function (promise) {
                   
                    $scope.employeedetailList = promise.employeedetailList;
                    
                });
        };

       


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.viewdocument = function (obj) {
            if (obj != null) {
                var img = obj.hrelaP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.hrelaP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.hrelaP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.hrelaP_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf').modal('show');
                        });
                }
            }
        };

        $scope.get_empdetails = function (obj) {
            var data = {
                "HRME_Id": obj.hrmE_Id
            };
            apiService.create("CumulativeSalaryReport/getemployeedetails", data).then(function (promise) {

                $scope.employeeDetails = promise.employeeDetails;
                $scope.HRME_EmployeeFirstName = $scope.employeeDetails[0].HRME_EmployeeFirstName;
                $scope.HRME_EmployeeCode = $scope.employeeDetails[0].HRME_EmployeeCode;
                $scope.HRME_DOJ = $scope.employeeDetails[0].HRME_DOJ;
                $scope.HRME_DOL = $scope.employeeDetails[0].HRME_DOL;
                $scope.HRMDES_DesignationName = $scope.employeeDetails[0].HRMDES_DesignationName;
                $scope.HRMD_DepartmentName = $scope.employeeDetails[0].HRMD_DepartmentName;
                $scope.MI_Name = $scope.employeeDetails[0].MI_Name;
                $scope.MI_Address1 = $scope.employeeDetails[0].MI_Address1;


                $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;

                });
        };

  

       

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();