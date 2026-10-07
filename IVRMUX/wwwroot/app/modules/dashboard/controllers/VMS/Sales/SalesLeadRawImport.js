(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesLeadImportController', SalesLeadImportController);
    SalesLeadImportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function SalesLeadImportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.filename = '';
        $scope.tblsh = false;
        var vm = this;
        vm.gridOptions = {};
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        var path = '../../../../../../DEMOSAMPLE.xls';
        var paginationformasters = 10;
        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=C:\Users\praveen\Downloads/DEMOSAMPLE.xls";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.components_list = [];
            $scope.Master_Employee_List = [];
            $scope.itemsPerPage = paginationformasters;
            var id = 2;
            apiService.getURI("ISM_Client_Project/getdata_MP", id).
                then(function (promise) {
                    if (promise.components_list != null && promise.components_list.length > 0) {
                        $scope.components_list = promise.components_list;
                    } if (promise.master_Employee_List != null && promise.master_Employee_List.length > 0) {
                        $scope.Master_Employee_List = promise.master_Employee_List;
                    }
                    
                });
        };

        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };

       
        $scope.savedata1 = function () {

        }
        $rootScope.validateForm = function () {
            //debugger;
            //console.log($scope.opts.data);
            //var data1 = {
            //    "newlstget1": $scope.opts.data
            //}
            //apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
            //    if (promise.stuStatus != "" && promise.stuStatus != null) {
            //        swal(promise.stuStatus);
            //        $elm.val(null);
            //        $scope.opts.data = null;
            //    } else {
            //        $scope.opts.data = $scope.opts.data;
            //    }
            //});
        };
        $scope.DuplicateDataArray = [];
        $scope.InvalidData = [];
        $scope.savedata = function (gridOptions) {
           // $scope.headers = ["LeadName", "LeadCode", "ContactPerson", "Designation", "ContactNo", "Email", "Category", "Source", "Reference", "Health", "Product", "StaffStrength", "StudentStrength", "TotalInstitutions", "Remark", "Address1", "Address2", "Address3", "Country", "State", "City", "VisitedDay", "VisitedMonth", "VisitedYear", "Pincode", "TelecallerEmpCode"];

           // $scope.headers = ["LeadName", "LeadCode", "ContactNo", "Email", "Address1", "Lead_Created_By", "Source", "Category", "Contact_Person", "Designation", "Health", "State", "Remarks"];


            $scope.headers = ["LeadName", "ContactNo", "Email", "LeadCode", "Address1", "Lead_Created_By", "Source", "Category", "Contact_Person", "Designation", "Health", "State", "Remarks"];



            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }
            }
            $scope.albumNameArray1 = [];
            angular.forEach($scope.albumNameArray[0], function (value, key) {
                //angular.forEach(value, function (value,key) {
                $scope.albumNameArray1.push(key);
                //});               
            });

            var excellvalidatecount = 0;

            angular.forEach($scope.headers, function (head1) {
                angular.forEach($scope.albumNameArray1, function (head2) {
                    if (head1 === head2) {
                        excellvalidatecount = excellvalidatecount + 1;
                    }
                });
            });

            if (excellvalidatecount == 13) {
                var data = {
                    "advimppln": $scope.albumNameArray,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    "message": "Rawimport"

                };
                apiService.create("SalesLeadImport/saveadvance", data).
                    then(function (promise) {
                        if (promise.message === "Record save Suceesfully !") {
                            $scope.DuplicateDataArray = promise.duplicateData;
                            $scope.InvalidData = promise.invalidData;
                            swal("Row Saved --" + promise.suscnt + " " + ",", "Duplicate " + promise.duplicateData.length + " ");
                        }
                        else if (promise.message === "Please Contact Administrator") {
                            $scope.DuplicateDataArray = promise.duplicateData;
                            $scope.InvalidData = promise.invalidData;
                            swal("Row Saved --" + promise.suscnt + " " + ",", "Duplicate " + promise.duplicateData.length + " ");
                        }
                        else {
                            swal(promise.message);
                        }
                    });
               
            }
            else {
                swal("Header Mismatch..!!", "Please Import a Excel With All/Proper Headers.");
            }
        };






        $scope.exportToExcel1 = function () {
            if ($scope.DuplicateDataArray.length > 0) {
                var exportHref = Excel.tableToExcel(exceldata, 'Duplicate Data');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };


        $scope.exportToExcelInvalidData = function () {
            if ($scope.InvalidData.length > 0) {
                var exportHref = Excel.tableToExcel(InvalidDataExcelData, 'Invalid Data');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };

        $scope.exportToExcel = function () {

            swal({
                title: "Are you sure?",
                text: "Do you want to Download the Duplicates Data into Excel",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        if ($scope.DuplicateDataArray.length > 0) {
                            
                            var exportHref = Excel.tableToExcel(exceldata, 'Duplicate Data');
                            $timeout(function () { location.href = exportHref; }, 100);
                            swal("Downloded Successfully.");
                           // swal.close();
                           // $state.reload();
                            
                        }
                       
                    }
                    else {
                        swal("Cancelled");
                       // $state.reload();
                    }
                   
                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //rawdata manual save//
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacteds = function (field) {
            return $scope.submitted11;
        };
        $scope.saveRawdataLead = function () {
            //$scope.submitted11 = true;
            //if ($scope.myFormNew.$valid) {


            //    var data = {
            //        "ISMSLERaw_LeadName": $scope.ISMSLERaw_LeadName,
            //        "ISMSLERaw_ContactNo": $scope.ISMSLERaw_ContactNo,
            //        "ISMSLERaw_EmailId": $scope.ISMSLERaw_EmailId,
            //        "ISMSLERaw_LeadAddress1": $scope.ISMSLERaw_LeadAddress1,
            //        "Source": $scope.Source,
            //        "Health": $scope.Health,
            //        "Contact_Person": $scope.Contact_Person,
            //        "Designation": $scope.Designation,
            //        "Remarks": $scope.Remarks,
            //        "Category": $scope.Category,
            //        "State": $scope.State,
            //        "Lead_Created_By": $scope.Lead_Created_By,
            //        "message": "Rawimportmanual"


            //    }

            //    apiService.create("SalesLeadImport/saveRawdataLead", data).then(function (promise) {
            //        if (promise.message == "Save") {
            //            swal('Record Saved Successfully!!!');
            //        }
            //        else if (promise.message != "NotSave" && promise.message != "Save") {
            //            swal(promise.message);
            //        }
            //        else if (promise.message == "NotSave") {
            //            swal("Record Not Saved");
            //        }
            //        else if (promise.message == "Error") {
            //            swal("Please contact Administrator !");
            //        }

            //        $state.reload();

            //    })
            //}
            //else {
            //    $scope.submitted11 = true;
            //}
            if ($scope.HRME_EmployeeCode != null && $scope.HRME_EmployeeCode != 0 && $scope.HRME_EmployeeCode != "" && $scope.HRME_EmployeeCode != undefined) {

                $scope.albumNameArray = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (emp) {
                        $scope.albumNameArray.push({
                            ISMSLERaw_LeadName: emp.ISMSLERaw_LeadName,
                            ISMSLERaw_ContactNo: emp.ISMSLERaw_ContactNo,
                            ISMSLERaw_EmailId: emp.ISMSLERaw_EmailId,
                            ISMSLERaw_LeadAddress1: emp.ISMSLERaw_LeadAddress1,
                            Source: emp.Source,
                            Health: emp.Health,
                            Designation: emp.Designation,
                            Remarks: emp.Remarks,
                            Category: emp.Category,
                            State: emp.State,
                            Contact_Person: emp.Contact_Person
                        })
                    });
                }


                if ($scope.albumNameArray.length > 0 && $scope.albumNameArray != null && $scope.albumNameArray != undefined) {
                    let isValid = true;  // Flag to check if any value is invalid

                    //angular.forEach($scope.albumNameArray, function (emp) {
                    //    if (!emp.ISMSLERaw_LeadName || !emp.ISMSLERaw_ContactNo || !emp.ISMSLERaw_EmailId ||
                    //        !emp.ISMSLERaw_LeadAddress1 || !emp.Source ||
                    //        !emp.Health || !emp.Designation || !emp.Remarks || !emp.Category || !emp.State) {
                    //        isValid = false;
                    //    }
                    //});


                    $scope.errorMessages = []; // To store error messages

                    angular.forEach($scope.albumNameArray, function (emp, index) {
                        var rowNumber = index + 1; // Row number starting from 1

                        // Check for mandatory fields (excluding email & phone)
                        if (!emp.ISMSLERaw_LeadName || !emp.ISMSLERaw_LeadAddress1 || !emp.Source ||
                            !emp.Health || !emp.Designation || !emp.Remarks || !emp.Category || !emp.State) {

                            $scope.isValid = false;
                            swal('Row ' + rowNumber + ': Please Enter All the data .');
                            return;
                            //$scope.errorMessages.push('Row ' + rowNumber + ': Please fill all mandatory fields.');
                        }

                        // Mobile number validation
                        var mobilePattern = /^[6-9]{1}[0-9]{9}$/;
                        if (!emp.ISMSLERaw_ContactNo) {
                            $scope.isValid = false;
                           // $scope.errorMessages.push('Row ' + rowNumber + ': Mobile number is required.');
                            swal('Row ' + rowNumber + ': Enter mobile number. ');
                            return;

                        } else if (!mobilePattern.test(emp.ISMSLERaw_ContactNo)) {
                            $scope.isValid = false;
                            swal('Row ' + rowNumber + ': Invalid mobile number. Please Enter Valid Mobile Number.');
                            //$scope.errorMessages.push('Row ' + rowNumber + ': Invalid mobile number. It should start with 6-9 and contain 10 digits.');
                            return;
                        }

                        // Email validation
                        var emailPattern = /^[A-Za-z0-9]+[A-Za-z0-9._]+@[A-Za-z0-9]+\.[A-Za-z.]{2,5}$/;
                        if (!emp.ISMSLERaw_EmailId) {
                            $scope.isValid = false;
                            swal('Row ' + rowNumber + ':Enter the Email ID.');
                            return;
                            //$scope.errorMessages.push('Row ' + rowNumber + ': Email ID is required.');
                        } else if (!emailPattern.test(emp.ISMSLERaw_EmailId)) {
                            $scope.isValid = false;
                            swal('Row ' + rowNumber + ': Invalid email address.');
                            return;
                            //$scope.errorMessages.push('Row ' + rowNumber + ': Invalid email address.');
                        }
                    });

                    if (!isValid) {
                        swal("Enter all the Valid Data");
                    } else {
                        $scope.submitted = true;
                        var data = {
                            "LeadDatasNew": $scope.albumNameArray,
                            "Lead_Created_By": $scope.HRME_EmployeeCode.HRME_EmployeeCode.toString(),
                            "message": "Rawimportmanual"
                        }
                        apiService.create("SalesLeadImport/saveRawdataLead", data).
                            then(function (promise) {
                    if (promise.message == "Save") {
                        swal('Record Saved Successfully!!!');
                    }
                    else if (promise.message != "NotSave" && promise.message != "Save") {
                        swal(promise.message);
                    }
                    else if (promise.message == "NotSave") {
                        swal("Record Not Saved");
                    }
                    else if (promise.message == "Error") {
                        swal("Please contact Administrator !");
                    }

                    $state.reload();
                            })

                    }
                }


            }
            else {
                swal('Please select the Employee..');
            }

        };

        //rawdata manual save//

        $scope.materaldocuupload = [{ id: 'Materal1' }];
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({
                    'id': 'Materal' + newItemNo,
                    ISMSLERaw_ExibitionDate: new Date()
                });
            }
        };
        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };


    }
})();
angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='
            },
            link: function ($scope, $elm, $attrs) {
                $elm.on('change', function (changeEvent) {
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;
                            var workbook = XLSX.read(data, { type: 'binary' });
                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];
                            data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length === 0) {
                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;
                            }
                            else {
                                $scope.headers = ["LeadName", "LeadCode", "ContactNo", "Email", "Address1", "Lead_Created_By", "Source", "Category", "Contact_Person", "Designation", "Health", "State","Remarks"];
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                $scope.checkheaders = [];
                                headerNames.forEach(function (h) {
                                    $scope.checkheaders.push(h);
                                });
                                var excellvalidatecount = 0;
                                $scope.Missinghead = [];
                                angular.forEach($scope.headers, function (head1) {
                                    var missingHead = 0;
                                    angular.forEach($scope.checkheaders, function (head2) {
                                        if (head1 === head2) {
                                            excellvalidatecount = excellvalidatecount + 1;
                                            missingHead = 1;
                                        }
                                    });
                                    if (missingHead === 0) {
                                        $scope.Missinghead.push(head1);
                                    }
                                });
                                if (excellvalidatecount == 13) {
                                    var bind = true;
                                    var excellcellvalidate = 0;
                                    $scope.fushlast = [];
                                    $scope.fushlast.push(0);
                                    angular.forEach(data, function (valu, ke) {
                                        $scope.rowheaders = [];
                                        angular.forEach(valu, function (val, ey) {
                                            $scope.rowheaders.push(ey);
                                        });
                                        var checkrow = 0;
                                        angular.forEach($scope.checkheaders, function (head1) {
                                            angular.forEach($scope.rowheaders, function (head2) {
                                                if (head1 === head2) {
                                                    checkrow = checkrow + 1;
                                                }
                                            });
                                        });
                                        if (checkrow !== $scope.checkheaders.length) {
                                            $scope.fushlast.push(valu.__rowNum__ + 1);
                                        }
                                    });
                                    if ($scope.fushlast.length === 1) {
                                        $scope.opts.data = data;
                                    }
                                    else {
                                        var Missingrows = "";
                                        if ($scope.fushlast.length > 1) {
                                            angular.forEach($scope.fushlast, function (head2) {
                                                if (head2 != 0) {
                                                    if (Missingrows === "") {
                                                        Missingrows = head2;
                                                    }
                                                    else {
                                                        Missingrows = Missingrows + "," + head2;
                                                    }
                                                }
                                            });
                                            Missingrows = "The Row " + Missingrows + " Contains Empty Cell Values, replace with the NULL for empty cell";
                                        }
                                        swal(Missingrows, "Excel Data is Not Proper!!");
                                    }
                                }
                                else {
                                    var Missingheads = "";
                                    if ($scope.Missinghead.length > 0) {
                                        angular.forEach($scope.Missinghead, function (head2) {
                                            if (head2 != 0) {
                                                if (Missingheads === "") {
                                                    Missingheads = head2;
                                                }
                                                else {
                                                    Missingheads = Missingheads + ", \n" + head2;
                                                }
                                            }
                                        });
                                        Missingheads = "The Missing Headers: \n" + Missingheads;
                                        swal(Missingheads, "Header Missing..!!");
                                    }
                                    else {
                                        swal("Header Missing..!!", "Please Import a Excel With All/Proper Headers.");
                                    }

                                }
                                
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);
                });
            }
        };
    }]);

