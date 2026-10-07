(function () {
    'use strict';
    angular
        .module('app')
        .controller('studentsearchController', studentsearchController)
    studentsearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function studentsearchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.searchValue = '';
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.grid_flag = false;
        $scope.tadprint = false;
        $scope.items = {};
        $scope.result = [
            {
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "AMST_FirstName", "name": "FirstName" },
                    { "value": "AMST_RegistrationNo", "name": "RegNo" },
                    { "value": "AMST_AdmNo", "name": "AdmissionNo" },
                    { "value": "PASR_Age", "name": "Age" },
                    { "value": "AMST_AadharNo", "name": "AadharNo" },
                    { "value": "AMST_MobileNo", "name": "MobileNo" },
                    { "value": "AMST_Sex", "name": "Sex" },
                    { "value": "AMST_Date", "name": "Date" },
                    { "value": "AMST_emailId", "name": "Email id" },
                    { "value": "AMST_FatherName", "name": "Father Name" },
                    { "value": "AMST_MotherName", "name": "Mother Name" },
                    { "value": "StudentName", "name": "Student Name" },
                    { "value": "ASMCL_ClassName", "name": "Class" },
                    { "value": "ASMC_SectionName", "name": "Section" },
                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" },
                ]

            }]
        $scope.addNew = function (index) {
            $scope['condflag' + index] = true;
            $scope.result.push({
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "AMST_FirstName", "name": "First Name" },
                    { "value": "AMST_RegistrationNo", "name": "Reg.No" },
                    { "value": "AMST_AdmNo", "name": "Admission No" },
                    { "value": "PASR_Age", "name": "Age" },
                    { "value": "AMST_AadharNo", "name": "AadharNo" },
                    { "value": "AMST_MobileNo", "name": "MobileNo" },
                    { "value": "AMST_Sex", "name": "Sex" },
                    { "value": "AMST_Date", "name": "Date" },
                    { "value": "AMST_emailId", "name": "Email id" },
                    { "value": "AMST_FatherName", "name": "Father Name" },
                    { "value": "AMST_MotherName", "name": "Mother Name" },
                    { "value": "StudentName", "name": "Student Name" },
                    { "value": "ASMCL_ClassName", "name": "Class" },
                    { "value": "ASMC_SectionName", "name": "Section" },
                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" }
                ]
            });
        };
        $scope.removeRow = function (index) {

            $scope.result.pop();
            $scope['condflag' + (index - 1)] = false;
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
            if ($scope.items.oprtr != '' && $scope.items.oprtr != null) {
                $scope.items.oprtr[index] = '';
            }
            if ($scope.items.field != '' && $scope.items.field != null) {
                $scope.items.field[index] = '';
            }
            if ($scope.items.conditn != '' && $scope.items.conditn != null) {
                $scope.items.conditn[index] = '';
            }
        }
        var abc = "";
        $scope.minall = abc;
        $scope.filterOperator = function (field, index) {


            if (field == "AMST_FirstName") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "AMST_RegistrationNo") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "AMST_AdmNo") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "AMST_AadharNo") {
                abc = "12";
                $scope.minall = abc;
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                    // { "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" }
                );
            }
            else if (field == "AMST_MobileNo") {
                abc = "10";
                $scope.minall = abc;
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                    // { "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" }
                );
            }

            else if (field == "AMST_Sex") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "AMST_Date") {
                swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" }
                );

            }
            else if (field == "PASR_Age") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" }
                );
            }
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.searchStud = function (inputs) {

            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var output = [];
                for (var key in inputs.field) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.field[key];
                    output.push(tempObj);
                }

                var output1 = [];
                for (var key in inputs.oprtr) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.oprtr[key];
                    output1.push(tempObj);
                }

                var output2 = [];
                for (var key in inputs.val) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.val[key];
                    output2.push(tempObj);
                }

                var output3 = [];
                for (var key in inputs.conditn) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.conditn[key];
                    output3.push(tempObj);
                }

                var data = {
                    output,
                    output1,
                    output2,
                    output3,
                    "stuStatus": $scope.radioValue
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StudentSearch/", data).
                    then(function (promise) {
                        if (promise.count == 0) {
                            swal("No Records Found.....!!");
                            $scope.grid_flag = false;
                            $state.reload();
                        }
                        else {
                            $scope.searchResult = promise.searchResult;
                            $scope.presentCountgrid = $scope.searchResult.length;
                            $scope.grid_flag = true;
                        }
                    })
            };
        }

        $scope.Clearid = function () {
            $state.reload();
            // $scope.radioValue = "";
            // $scope.items = "";
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.searchResult, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }
        $scope.exportToExcel = function (tableId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please select records to be exported");
            }

        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }


        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });
    }
})
    ();