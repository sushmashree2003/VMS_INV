(function () {
    'use strict';
    angular
        .module('app')
        .controller('Classwisestudentconteoller', Classwisestudentconteoller)

    Classwisestudentconteoller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function Classwisestudentconteoller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.searchValue = '';

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.grid_flag = false;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.printstudents = [];

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            var pageid = 1;
            apiService.getURI("Classwisestudentconteoller/getalldetails", pageid).
                then(function (promise) {
                    // $scope.start_yr = promise.fillyear[0].asmaY_Year;
                    $scope.yearlst = promise.fillyear;
                    $scope.arrclasslist = promise.fillclass;
                    //     $scope.arrseclist = promise.fillsection;
                    //  $scope.arrlistchkhead = promise.alldata;
                    $scope.albumNameArraycolumn = [];
                    $scope.columnsTest = [];
                    $scope.headertest = [
                        { id: 'AMST_FirstName', checked: true, value: 'Student Name' },
                        { id: 'AMST_PerAdd3', checked: true, value: 'Permanent Address' },
                        { id: 'AMST_ConCity', checked: true, value: 'Present Address' },
                        { id: 'AMST_RegistrationNo', checked: true, value: 'Reg. No.' },
                        { id: 'JoinedYear', checked: true, value: 'JoinedYear' },
                        { id: 'JoinedClass', checked: true, value: 'JoinedClass' },
                        { id: 'AMST_Sex', checked: true, value: 'Gender' },
                        { id: 'AMST_BloodGroup', checked: true, value: 'Blood Group' },
                        { id: 'AMST_MotherTongue', checked: true, value: 'Mother Tongue' },
                        { id: 'AMST_MobileNo', checked: true, value: 'Phone No.' },
                        { id: 'AMST_emailId', checked: true, value: 'Email ID' },
                        { id: 'IVRMMC_CountryName', checked: true, value: 'Nationality' },
                        { id: 'PASR_Age', checked: true, value: 'Age' },
                        { id: 'AMST_Photoname', checked: true, value: 'Student Photo' },
                        { id: 'IMC_CasteName', checked: true, value: 'Caste' },
                        { id: 'IMCC_CategoryName', checked: true, value: 'Caste Category' },
                        { id: 'AMC_Name', checked: true, value: 'Category' },
                        { id: 'IVRMMR_Name', checked: true, value: 'Religion' },
                        { id: 'AMST_AdmNo', checked: true, value: 'Adm. No.' },
                        { id: 'AMAY_RollNo', checked: true, value: 'Roll No.' },
                        { id: 'ASMCL_ClassName', checked: true, value: 'Class' },
                        { id: 'ASMC_SectionName', checked: true, value: 'Section' },
                        { id: 'AMST_Date', checked: true, value: 'DOA' },
                        { id: 'AMST_DOB', checked: true, value: 'DOB' },
                        { id: 'AMST_DOB_Words', checked: true, value: "DOB(Words)" },
                        { id: 'AMST_PerStreet', checked: true, value: 'Street' },
                        { id: 'AMST_PerCity', checked: true, value: 'Town' },
                        { id: 'IVRMMS_Name', checked: true, value: 'State' },
                        { id: 'AMST_FatherName', checked: true, value: 'Father Name' },
                        { id: 'AMST_FatherMobleNo', checked: true, value: 'Father Mobile Number' },
                        { id: 'AMST_FatheremailId', checked: true, value: 'Father Email Id' },
                        { id: 'AMST_FatherOccupation', checked: true, value: 'Father Occupation' },
                        { id: 'AMST_FatherMonIncome', checked: true, value: 'Father Income' },
                        { id: 'AMST_FatherEducation', checked: true, value: 'Father Qualification' },
                        { id: 'AMST_MotherName', checked: true, value: 'Mother Name' },
                        { id: 'AMST_MotherMobileNo', checked: true, value: 'Mother Mobile Number' },
                        { id: 'AMST_MotherEmailId', checked: true, value: 'Mother Email Id' },
                        { id: 'AMST_MotherOccupation', checked: true, value: 'Mother Occupation' },
                        { id: 'AMST_MotherMonIncome', checked: true, value: 'Mother’s Income' },
                        { id: 'AMST_MotherEducation', checked: true, value: 'Mother Qualification' },
                        { id: 'AMST_AadharNo', checked: true, value: 'Aadhar Card Number' },                       
                        { id: 'AMST_Tpin', checked: true, value: 'Tpin' },
                        { id: 'AMST_GovtAdmno', checked: true, value: 'GovtAdmno' },
                        { id: 'AMST_BPLCardNo', checked: true, value: 'GovtNo' },
                        { id: 'AMST_BirthPlace', checked: true, value: 'Place Of Birth' },
                        { id: 'AMSTPS_PrvSchoolName', checked: true, value: 'Previous School Name' },
                        { id: 'AMSTPS_PreviousClass', checked: true, value: 'Previous Class' },
                    ]
                    // 
                })
        }

        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];

        $scope.getsection = function () {

            var data = {
                "ASMCL_Id": $scope.ASMCL,
                "ASMAY_Id": $scope.ASMAY
            }
            apiService.create("Classwisestudentconteoller/getsection", data).then(function (promise) {

                if (promise != null) {
                    $scope.arrseclist = promise.fillsection;
                }
                else {
                    swal("No Record Found");
                }
            })
        }

        $scope.addColumn = function (role) {
            //for unselect the header check box
            $scope.all2 = $scope.headertest.every(function (itm) { return itm.selected; });
            //

            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
                //var newCol = { id: role.value, checked: true, value: role.id }
                //  $scope.columnsTest.push(newCol);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                // $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.value).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.submitted = false;
        $scope.ShowReport = function () {

            $scope.printstudents = [];
            $scope.search = "";
            $scope.all = "";
            $scope.albumNameArraycolumn = [];
            angular.forEach($scope.headertest, function (role) {
                if (!!role.selected) $scope.albumNameArraycolumn.push({
                    columnID: role.id,
                    columnName: role.value
                });
            })

            //if ($scope.TC_allorind == "all") {
            //    $scope.ASMC = '0';
            //    $scope.ASMCL = '0';
            //}
            //else if ($scope.TC_allorind === "indi") {
            //    $scope.ASMCL = $scope.ASMCL;
            //    $scope.ASMC = $scope.ASMC;
            //}

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY,
                    //"allorindiflag": $scope.TC_allorind,
                    "ASMCL_Id": $scope.ASMCL,
                    "ASMC_Id": $scope.ASMC,
                    //"tcallorindi": $scope.admstdpro,
                    "TempararyArrayheadList": $scope.albumNameArraycolumn,
                    "flag": $scope.admstdpro,
                }
                apiService.create("Classwisestudentconteoller/Getreportdetails", data).
                    then(function (promise) {

                        if (promise.alldatagridreport.length > 0) {
                            $scope.students = promise.alldatagridreport;
                            $scope.presentCountgrid = $scope.students.length;
                            angular.forEach($scope.students, function (objectt) {
                                if (objectt.AMST_FirstName != undefined) {
                                    var string = objectt.AMST_FirstName
                                    objectt.AMST_FirstName = string.replace(/  +/g, ' ');
                                }
                                if (objectt.AMST_FatherOccupation != '0' && objectt.AMST_FatherOccupation != undefined) {

                                    objectt.AMST_FatherOccupation = objectt.AMST_FatherOccupation;
                                } else {
                                    objectt.AMST_FatherOccupation='Not Available'
                                }

                                if (objectt.AMST_MotherOccupation != '0' && objectt.AMST_MotherOccupation != undefined) {

                                    objectt.AMST_MotherOccupation = objectt.AMST_MotherOccupation;
                                } else {
                                    objectt.AMST_MotherOccupation = 'Not Available'
                                }

                                if (objectt.AMST_FatherEducation != '0' && objectt.AMST_FatherEducation != undefined) {

                                    objectt.AMST_FatherEducation = objectt.AMST_FatherEducation;
                                } else {
                                    objectt.AMST_FatherEducation = 'Not Available'
                                }

                                if (objectt.AMST_MotherEducation != '0' && objectt.AMST_MotherEducation != undefined) {

                                    objectt.AMST_MotherEducation = objectt.AMST_MotherEducation;
                                } else {
                                    objectt.AMST_MotherEducation = 'Not Available'
                                }

                                if (objectt.AMST_BirthPlace != '0' && objectt.AMST_BirthPlace != undefined) {

                                    objectt.AMST_BirthPlace = objectt.AMST_BirthPlace;
                                } else {
                                    objectt.AMST_BirthPlace = 'Not Available'
                                }
                                

                            })
                            // $scope.imgname = promise.photopathname;
                            $scope.columnsTest = promise.tempararyArrayheadList;
                            //console.log($scope.columnsTest);
                            $scope.grid_flag = true;
                            $scope.showbutton = true;

                            angular.forEach($scope.arrclasslist, function (cls) {
                                if (cls.asmcL_Id == $scope.ASMCL) {
                                    $scope.class = cls.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.arrseclist, function (clse) {
                                if (clse.asmS_Id == $scope.ASMC) {
                                    $scope.section = clse.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.yearlst, function (clse) {
                                if (clse.asmaY_Id == $scope.ASMAY) {
                                    $scope.yearname = clse.asmaY_Year;
                                }
                            });
                            
                        }
                        else {
                            $scope.showbutton = false;
                            $scope.grid_flag = false;
                            $scope.students = "";
                            swal("No Records Found !!");
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Classwisestudentconteoller/1", data).
                then(function (promise) {
                    $scope.students = promise.alldatagridreport;
                    swal("searched Sucessfully");
                })
        }

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.selected;
            });
        }

        //Toggle all header check box
        $scope.Toggle_header = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.headertest, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        //for export to excel
        $scope.exportToExcel = function (export_id) {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }

        //for print     
        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
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

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.clear = function () {
            $state.reload();
            $scope.grid_flag = false;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();

