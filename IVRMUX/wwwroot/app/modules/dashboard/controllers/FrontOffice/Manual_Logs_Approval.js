(function () {
    'use strict';
    angular
        .module('app')
        .controller('ManualLogsApprovalController', ManualLogsApprovalController);

    ManualLogsApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function ManualLogsApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;
        $scope.verifiedall = false;
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Employee_Add_logs_Manual/approvalloaddata", pageid).then(function (promise) {

                if (promise.punchapproveddetails !== null && promise.punchapproveddetails.length > 0) {
                    $scope.punchapproveddetails = promise.punchapproveddetails;
                }

                if (promise.punchdetails !== null && promise.punchdetails.length > 0) {
                    $scope.punchdetails = promise.punchdetails;
                }

                angular.forEach($scope.punchapproveddetails, function (item) {
                    item.FOEPARD_Time = moment(item.FOEPARD_Time, 'H:mm ').format();
                });

            });
        };



        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            $scope.logapprovallist = [];
            $scope.verifiedall = false;
            var checkStatus2 = $scope.alltask;
            if (checkStatus2 == true) {
                angular.forEach($scope.punchapproveddetails, function (itm) {
                    $scope.Missingtimedetailsall(itm);
                    itm.selectedd2 = checkStatus2;
                });
            }
            else {
                angular.forEach($scope.punchapproveddetails, function (itm) {
                    itm.selectedd2 = checkStatus2;
                });
                $scope.logapprovallist = [];
            }

            if (checkStatus2 == true && $scope.logapprovallist.length > 0) {
                $('#inoutdetailsforapproval').modal('show');
            }
            else {
                $('#inoutdetailsforapproval').modal('hide');
            }


        };

        $scope.Missingtimedetailsall = function (obj) {
            $scope.logapprovallist = [];

            var json = "0";
            var checkStatus2 = $scope.alltask;
            if (checkStatus2 == true) {
                angular.forEach($scope.punchapproveddetails, function (itm) {
                    json = json + "," + itm.FOEPAR_Id;
                    itm.selectedd2 = checkStatus2;
                });


                var data = {
                    "FOEPAR_Id": 0,
                    "json": json
                };
                apiService.create("Employee_Add_logs_Manual/FindData", data).
                    then(function (promise) {
                        if (promise.logapprovallist !== null && promise.logapprovallist.length > 0) {
                            $scope.logapprovallist1 = promise.logapprovallist;
                            angular.forEach($scope.logapprovallist1, function (item1) {                              
                                item1.FOEPARD_Time = $filter('date')(item1.FOEPARD_Time, "HH:mm");
                                $scope.logapprovallist.push(item1);
                            });

                            if (checkStatus2 == true && $scope.logapprovallist.length > 0) {
                                $('#inoutdetailsforapproval').modal('show');
                            }
                            else {
                                $('#inoutdetailsforapproval').modal('hide');
                            }

                        }
                        else {
                            swal("No Manual Logs Found !!");
                            return;
                        }
                    });
            }
            else {
                angular.forEach($scope.punchapproveddetails, function (itm) {
                    itm.selectedd2 = checkStatus2;
                });
                $scope.logapprovallist = [];
            }
        };




        $scope.cancel = function () {
            $scope.hrmlY_LeaveYear = "";
            $scope.ivrM_Month_Id = "";
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.viewdocument = function (obj) {
            if ($scope.myForm1.$valid) {
                var data = {
                    "FOEPAR_Id": obj.FOEPAR_Id,
                    "json": ""
                };
                apiService.create("Employee_Add_logs_Manual/FindData", data).
                    then(function (promise) {
                        if (promise.logapprovallist !== null && promise.logapprovallist.length > 0) {
                            $scope.logapprovallist1 = promise.logapprovallist;
                            angular.forEach($scope.logapprovallist1, function (item) {
                                item.FOEPARD_Time = $filter('date')(item.FOEPARD_Time, "HH:mm");
                            });
                            $scope.EmployeeDis = true;
                            $('#inoutdetails').modal('show');
                        }
                        else {
                            swal("No Manual Logs Found !!");
                            return;
                        }
                    });
            }
            else { $scope.submitted = true; }
        };


        $scope.closeimage = function (obj) {

            $('#myModal').modal('hide');

            //$('#inoutdetailsforapproval').modal('hide');
            $('.modal-backdrop').remove();
          //  $('#inoutdetailsforapproval').modal('show');


        }
        $scope.Missingtimedetails = function (obj) {

            $scope.verifiedall = false;
            if ($scope.myForm1.$valid) {
                var data = {
                    "FOEPAR_Id": obj.FOEPAR_Id,
                    "json": ""
                };
                apiService.create("Employee_Add_logs_Manual/FindData", data).
                    then(function (promise) {
                        if (promise.logapprovallist !== null && promise.logapprovallist.length > 0) {
                            $scope.logapprovallist = promise.logapprovallist;
                            angular.forEach($scope.logapprovallist, function (item) {
                                item.FOEPARD_Time = moment(item.FOEPARD_Time, 'H:mm ').format();
                            });
                            $scope.EmployeeDis = true;
                            if (obj.selectedd2 == false) {
                                $('#inoutdetailsforapproval').modal('hide');
                            }
                            else {
                                $('#inoutdetailsforapproval').modal('show');
                            }

                        }
                        else {
                            swal("No Manual Logs Found !!");
                            return;
                        }
                    });
            }
            else { $scope.submitted = true; }
        };


        $scope.verifylogs = function (obj) {
            var checkStatus3 = $scope.verifiedall;
            angular.forEach($scope.logapprovallist, function (item) {
                item.verified = checkStatus3;
            });
        };


        $scope.removefromcart = function (index) {
            var newItemNo = $scope.addtocartdetails.length - 1;
            $scope.addtocartdetails.splice(index, 1);
        };

        $scope.saveapproval = function (status) {
            if ($scope.myForm1.$valid) {
                $scope.temp_array2 = [];
                
              
                angular.forEach($scope.logapprovallist, function (emp) {
                    if (emp.verified == true) {
                        $scope.temp_array2.push({ FOEPAR_Remarks: emp.ARemarks, FOEPD_PunchTime: $filter('date')(emp.FOEPARD_Time, "HH:mm"), FOEPARD_InOutFlg: emp.FOEPARD_InOutFlg, FOEPARD_Id: emp.FOEPARD_Id, Status: status });
                    }               
                });
                if ($scope.temp_array2.length  == 0) {
                    swal("Kindly select any checkbox");
                    return;
                }
                var data = {
                    "temp1": $scope.temp_array2
                };
                apiService.create("Employee_Add_logs_Manual/saveapproval", data).then(function (promise) {

                    if (promise.returnmsg !== "") {
                        swal("Records Saved Successfully..");
                        $state.reload();                      
                        //if (promise.returnmsg === "Success") {
                        //    swal("Records Approved Successfully..");
                        //    $state.reload();
                        //}
                        //else if (promise.returnmsg === "NotSuccess") {
                        //    swal("Approval Failed!, Kindly contact Administrator ");
                        //}
                        //else {
                        //    swal("Something went wrong ..!", 'Kindly contact Administrator');
                        //}
                    }
                    $state.reload();
                });
            }
        };



        $scope.showmodaldetailsBOSBOE = function (file) {
            //$('#preview').removeAttr('src');
            var filename = file.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', file);
                $('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', file);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = file;
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



        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'CheckListDetails');
            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }

})();