(function () {
    'use strict';

    angular
        .module('app')
        .controller('CollegeAdmissionForm', CollegeAdmissionForm);

    CollegeAdmissionForm.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter'];

    function CollegeAdmissionForm($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter) {



        $scope.zoomin = function () {

            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        }
        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        }

        var monthname;
        var datename;
        var yearname;
        $scope.search = "";
        $scope.AMCST_Date = new Date();
        $scope.sortKey = 'amcsT_Id';
        $scope.sortReverse = true;


        $scope.address = true;
        $scope.Parents = true;
        $scope.Others = true;
        $scope.DocumentUpload = true;

        var maxageeyear;
        var maxageemonth;
        var maxageedays;
        var minageeyear;
        var minageemonth;
        var minageedays;

        $scope.studentGuardianDetails = {};
        $scope.studentReferenceDetails = [];
        $scope.studentSourceDetails = [];
        $scope.studentActivityDetails = [];

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {

            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
            else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }


        $scope.itemsPerPage = paginationformasters;

        $scope.myDate = new Date();
        //calculating age , confirmation of age if there is any lessthan and greather than, and converting DOB in words
        $scope.calcage = function (dateString, SweetAlert) {

            angular.forEach($scope.allClass, function (c) {
                if (c.asmcL_Id == $scope.obj.asmcL_Id) {
                    $scope.asmcL_ClassName = c.asmcL_ClassName;
                }
            })

            $scope.classname11 = $scope.asmcL_ClassName;
            //converting date into words			
            var dobwords = new Date(dateString);
            var monthid = dobwords.getMonth();
            var dateid = dobwords.getDate();
            var yearid = dobwords.getFullYear();
            $scope.getmontnames(monthid);
            $scope.getdatenames(dateid);
            $scope.getyearname(yearid);
            $scope.obj.AMCST_DOBin_words = datename + ' ' + monthname + ' ' + yearname;
            // $scope.dobwords = false;

            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date(dateString);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };

            // swal(ageString);
            $scope.obj.AMCST_Age = age.years;

            if ($scope.minage == 1) {
                if (age.years > minageeyear) {
                }
                else if (age.years == minageeyear) {
                    if (age.months > minageemonth) {

                    }
                    else if (age.months == minageemonth) {
                        if (age.days >= minageedays) {

                        }
                        else {

                            swal({
                                title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {

                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {

                        swal({
                            title: "Less than " + minageeyear + " year ,You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {

                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";
                                });
                            }
                        });
                    }
                }
                else {

                    swal({
                        title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {

                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }

            if ($scope.maxage == 1) {
                if (age.years < maxageeyear) {
                }
                else if (age.years == maxageeyear) {
                    if (age.months < maxageemonth) {

                    }
                    else if (age.months == maxageemonth) {
                        if (age.days <= maxageedays) {

                        }
                        else {


                            swal({
                                title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {

                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {


                        swal({
                            title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {

                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";

                                });
                            }
                        });
                    }
                }
                else {


                    swal({
                        title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {

                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }
        }
        $scope.DOB = true;
        this.minDate = new Date();
        $scope.onYearCahnge = function (acdYId) {
            apiService.getURI("CollegeStudentAdmission/getCourse/", acdYId).then(function (promise) {

                if (promise.courses != null) {
                    $scope.courses = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.courses = "";
                }
            });
        }
        //checking min and max age details
        $scope.onBranchchange = function (branchId) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeStudentAdmission/getSemester/", data).then(function (promise) {

                    if (promise.message == "MaxCapacity") {
                        swal("Sorry,Branch Capacity is Full");
                        // $scope.applastdatedisable = true;
                    }
                    else {
                        //  $scope.applastdatedisable = false;
                        //  if (promise.message != "" && promise.message != null) {
                        //      swal(promise.message);
                        //  }
                        //  else {
                        if (promise.semesters != null) {
                            $scope.semesters = promise.semesters;
                        }
                        else {
                            swal("No Semester Is Mapped To Selected Branch");
                            $scope.semesters = "";
                        }

                        // }
                    }
                    //if (flg == 0) {
                    $scope.DOB = false;
                    //}
                    //$scope.obj.AMCOC_Id = "";
                    //if (promise.studentCategory.length > 0) {
                    //    $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    //}
                    //else {
                    //    swal("To get Student Category.Please Map Selected Class to Some category");
                    //}


                    //maxageeyear = promise.fillclass[0].asmcL_MaxAgeYear;
                    //maxageemonth = promise.fillclass[0].asmcL_MaxAgeMonth;
                    //maxageedays = promise.fillclass[0].asmcL_MaxAgeDays;
                    //minageeyear = promise.fillclass[0].asmcL_MinAgeYear;
                    //minageemonth = promise.fillclass[0].asmcL_MinAgeMonth;
                    //minageedays = promise.fillclass[0].asmcL_MinAgeDays;
                    //if (flg == 0) {
                    //    if ($scope.maxage == 0 && $scope.minage == 0) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 1 && $scope.minage == 0) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 0 && $scope.minage == 1) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 1 && $scope.minage == 1) {

                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //}

                })

            }
        }

        $scope.onCourseChange = function (courseId) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ACAYC_Id": selectedData[0].acayC_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeStudentAdmission/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branches = "";
                    }
                })
            }
        }
        $scope.getQuotaCategory = function (quotaId) {
            if (quotaId > 0) {
                apiService.getURI("CollegeStudentAdmission/getQuotaCategory/", quotaId).then(function (promise) {
                    if (promise.quotaCategory != null) {
                        $scope.quotaCategory = promise.quotaCategory;
                    }
                    else {
                        swal("No Quota Category Is Mapped To Selected Quota");
                    }
                });
            }
        }
        $scope.myTabIndex = 0;

        $scope.obj = {};
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //student form validation


        $scope.submitted1 = false;
        $scope.isduplicate = false;

        //address form validation
        $scope.submitted2 = false;
        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }
        //parents form validation
        $scope.submitted3 = false;
        $scope.Parents_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        //others form validation
        $scope.submitted4 = false;
        $scope.validateOtherPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.previous_document = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {

            return $scope.submitted3 || field.$dirty;
        };
        $scope.interacted4 = function (field) {

            return $scope.submitted4 || field.$dirty;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.studentSiblingDetails = [{ id: 'sibling1' }];
        $scope.addNewsibling = function () {
            var newItemNo = $scope.studentSiblingDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.studentSiblingDetails.push({ 'id': 'sibling' + newItemNo });
            }
        };

        $scope.removeNewsibling = function (index) {
            var newItemNo = $scope.studentSiblingDetails.length - 1;
            $scope.studentSiblingDetails.splice(index, 1);

            if ($scope.studentSiblingDetails.length == 0) {
            }
        };

        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.studentGuardianDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.studentGuardianDetails.push({ 'id': 'Guardian' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.studentGuardianDetails.length - 1;
            $scope.studentGuardianDetails.splice(index, 1);

            if ($scope.studentGuardianDetails.length == 0) {
            }
        };

        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
        $scope.addNewsiblingprevsch = function () {
            var newItemNo = $scope.prevSchoolDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.prevSchoolDetails.push({ 'id': 'prevSchool' + newItemNo });
            }
        };

        $scope.removeNewsiblingprevsch = function (index, data) {
            var newItemNo = $scope.prevSchoolDetails.length - 1;
            $scope.prevSchoolDetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.prevSchoolDetails.length == 0) {
            }
        };


        // Previous Qualify Exam Marks details

        $scope.prevexammarksdetails = [{ id: 'prevExamdetails1' }];
        $scope.addNewsiblingprevschexamdetails = function () {
            var newItemNo = $scope.prevexammarksdetails.length + 1;

            if (newItemNo <= 20) {
                $scope.prevexammarksdetails.push({ 'id': 'prevExamdetails' + newItemNo });
            }
        };

        $scope.removeNewsiblingprevschexamdetails = function (index, data) {
            var newItemNo = $scope.prevexammarksdetails.length - 1;
            $scope.prevexammarksdetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.prevexammarksdetails.length == 0) {
            }
        };

        // End 







        $scope.allActivity = [];
        $scope.allSources = [];
        $scope.allRefrence = [];
        $scope.RegistrationNumbering = [];
        $scope.AdmissionNumbering = [];
        $scope.govtBondList = [];
        //Get data Initially
        $scope.BindData = function () {
            apiService.getDATA("CollegeStudentAdmission/Getdetails").
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.statelabel = true;
                    $scope.statelabel2 = true;


                    $scope.allAcademicYear = promise.academicYearOnLoad;
                    $scope.allAcademicYear1 = promise.allAcademicYear;

                    $scope.allAcademicYearsearch = promise.academicYearOnLoad;
                    $scope.allAcademicYear1search = promise.allAcademicYear;

                    //previous  last year
                    $scope.previouspassyear = promise.academicYearOnLoad;


                    for (var i = 0; i < $scope.allAcademicYear.length; i++) {
                        name = $scope.allAcademicYear[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.allAcademicYear[i].Selected = true;
                                $scope.obj.ASMAY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                                $scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                            }
                        }
                    }
                    //  $scope.yearre = promise.academicyearforreadmit;

                    //for (var i = 0; i < $scope.allAcademicYearsearch.length; i++) {
                    //    name = $scope.allAcademicYearsearch[i].acmaY_Id;
                    //    for (var j = 0; j < $scope.allAcademicYear1search.length; j++) {
                    //        if (name == $scope.allAcademicYear1search[j].acmaY_Id) {
                    //            $scope.allAcademicYearsearch[i].Selected = true;
                    //            $scope.obj.asmaY_Idsearch = $scope.allAcademicYear1search[j].acmaY_Id;

                    //        }
                    //    }
                    //}
                    $scope.branches = promise.branches;
                    $scope.courses = promise.courses;



                    $scope.semesters = promise.semesters;
                    //previous class
                    // $scope.branches = promise.branches;
                    // $scope.allSection = promise.allSection;
                    $scope.allcountry = promise.allCountry;
                    //previous country
                    $scope.allcountry1 = promise.allCountry;
                    $scope.allReligion = promise.allReligion;
                    $scope.allReligionfather = promise.allReligion;
                    $scope.allReligionmother = promise.allReligion;
                    $scope.allcasteCategory = promise.allcasteCategory;
                    $scope.allCaste = promise.allCaste;
                    $scope.allCastefather = promise.allCaste;
                    $scope.allCastemother = promise.allCaste;
                    $scope.allRefrence = promise.allRefrence;
                    $scope.allSources = promise.allSources;
                    $scope.batches = promise.batches;
                    $scope.quotas = promise.quotas;
                    $scope.quotaCategory = promise.quotaCategory;
                    $scope.subjectScheme = promise.subjectScheme;
                    $scope.schemeType = promise.schemeType;
                    $scope.StudentList = promise.studentList;
                    $scope.presentCountgrid = $scope.StudentList.length;
                    $scope.allActivity = promise.allActivity;
                    $scope.studentCategory = promise.studentCategory;

                    $scope.allState = promise.allState_new;


                    $scope.siblingCourse = promise.courses;

                    //document list

                    $scope.documentList = promise.documentList;
                    angular.forEach($scope.documentList, function (value1, i) {
                        $scope.documentList[i].document_Path = "";
                    });


                    //admissionconfiguration code starts.

                    //  var admissioncongiguration = promise.admissioncongigurationList;
                    //  localStorage.setItem("admissionconfigsettings", JSON.stringify(admissioncongiguration));

                    //var admissionSettings = JSON.parse(localStorage.getItem("admissionconfigsettings"));
                    //
                    //if (admissionSettings != null && admissionSettings != "") {
                    //    if (admissionSettings.length > 0) {
                    //        $scope.maxage = admissionSettings[0].asC_MaxAgeApl_Flag;
                    //        $scope.minage = admissionSettings[0].asC_MinAgeApl_Flag;
                    //        $scope.gender = admissionSettings[0].asC_Default_Gender;
                    //        $scope.parents_m_income = admissionSettings[0].asC_ParentsMonthlyIncome_Flag;
                    //        $scope.parents_a_income = admissionSettings[0].asC_ParentsAnnualIncome_Flag;
                    //        $scope.default_sms_email = admissionSettings[0].asC_DefaultSMS_Flag;
                    //        $scope.photo = admissionSettings[0].asC_DefaultPhotoUpload;
                    //    }
                    //}
                    //else {
                    //    $scope.gender = 0;
                    //    $scope.maxage = 0;
                    //    $scope.minage = 0;
                    //    $scope.parents_m_income = 0;
                    //    $scope.parents_a_income = 0;
                    //    $scope.default_sms_email = 0;
                    //    $scope.photo = 0;
                    //}

                    //
                    //if ($scope.gender == "M") {
                    //    $scope.amsT_Sex = 'Male';
                    //}
                    //else if ($scope.gender == "F") {
                    //    $scope.amsT_Sex = 'Female';
                    //}
                    //else if ($scope.gender == "Ot") {
                    //    $scope.amsT_Sex = 'Other';
                    //}
                    //else {
                    //    $scope.amsT_Sex = '';
                    //}
                    //if ($scope.parents_m_income == 1) {
                    //    $scope.parents_monthly_income = 1;
                    //}
                    //else {
                    //    $scope.parents_monthly_income = 0;
                    //}
                    //if ($scope.parents_a_income == 1) {
                    //    $scope.parents_annual_income = 1;
                    //}
                    //else {
                    //    $scope.parents_annual_income = 0;
                    //}
                    //if ($scope.photo == 1) {
                    //    $scope.profile_photo = 1;
                    //}
                    //else {
                    //    $scope.profile_photo = 2;
                    //}
                    ////admissionconfiguration code ends.

                    ////Getting Transaction Numbering.
                    //
                    var transnumconfig = promise.admTransNumSetting;
                    localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
                    var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

                    for (var i = 0; i < transnumconfigsettings.length; i++) {
                        if (transnumconfigsettings.length > 0) {
                            $scope.transnumbconfigurationsettings = transnumconfigsettings[i];

                            if (transnumconfigsettings[i].imN_Flag == "AdmissionReg") {
                                $scope.RegistrationNumbering = transnumconfigsettings[i];
                                if (transnumconfigsettings[i].imN_AutoManualFlag == "Manual") {
                                    $scope.reg_ = true;
                                    $scope.regpreventduplicateflag = transnumconfigsettings[i].imN_DuplicatesFlag;
                                }
                                else {
                                    $scope.reg_ = false;
                                }
                            }
                            if (transnumconfigsettings[i].imN_Flag == "Admission") {
                                $scope.AdmissionNumbering = transnumconfigsettings[i];
                                if (transnumconfigsettings[i].imN_AutoManualFlag == "Manual") {
                                    $scope.Adm_ = true;
                                    $scope.admpreventduplicateflag = transnumconfigsettings[i].imN_DuplicatesFlag;
                                }
                                else {
                                    $scope.Adm_ = false;
                                }
                            }
                        }
                    }
                    ////Getting Transaction Numbering.
                    if (promise.studentList != null && promise.studentList != "") {
                        for (var i = 0; i < promise.studentList.length; i++) {
                            if (promise.studentList[i].amcsT_MiddleName == null || promise.studentList[i].amcsT_MiddleName == '' || promise.studentList[i].amcsT_MiddleName == "") {
                                promise.studentList[i].amcsT_MiddleName = "";
                            }

                            if (promise.studentList[i].amcsT_LastName == null || promise.studentList[i].amcsT_LastName == '' || promise.studentList[i].amcsT_LastName == "") {
                                promise.studentList[i].amcsT_LastName = "";
                            }

                            if (promise.studentList[i].amcsT_SOL == "S") {
                                promise.studentList[i].amcsT_SOL = "Active";
                            }
                            else if (promise.studentList[i].amcsT_SOL == "D") {
                                promise.studentList[i].amcsT_SOL = "Deactive";
                            }
                            else if (promise.studentList[i].amcsT_SOL == "L") {
                                promise.studentList[i].amcsT_SOL = "Left";
                            }
                        }

                    }


                    //if (promise.govtBondList != null && promise.govtBondList != "") {
                    //    $scope.govtBondList = promise.govtBondList;
                    //    angular.forEach($scope.govtBondList, function (value1, i) {
                    //        $scope.govtBondList[i].amstB_BandNo = "";
                    //    });
                    //}
                    if (promise.boardList != null && promise.boardList != "") {
                        $scope.boardList = promise.boardList;
                    }

                    ////fee concession list.
                    //if (promise.concessionList != null && promise.concessionList != "") {
                    //    $scope.concessionList = promise.concessionList;
                    //}
                    //else {
                    //    swal("Fee Concession Is Not Created For This Institute.Please Create Fee Concession Using Fee Master Concession Page In Fees Module And Then Proceed.");
                    //}
                    //School type.
                    $scope.Schooltypelist = promise.schooltypelist;

                    //Previous School Class.
                    //   $scope.prevSchlCls = promise.courses;

                    $scope.prevSchlCls = promise.combinationlist;

                    //Previous School Year.
                    $scope.prevYr = promise.academicYearOnLoad;

                    //Previous School Country.
                    $scope.prevCountry = promise.allCountry;

                })
        };


        //get Student Category list
        $scope.GetStudentCategory = function (classId) {
            if (classId != "" && classId != null && classId != undefined) {
            }
        }

        $scope.DeleteBondData = function (DeleteRecord, SweetAlert) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.deleteId = DeleteRecord.amstB_Id;
                        var MdeleteId = $scope.deleteId;
                        apiService.DeleteURI("StudentAdmission/DeleteBondEntry", MdeleteId)

                        swal("Record Deleted Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.DeletePrevSchoolData = function (DeleteRecord) {
            var confirmPopup = confirm('Are you sure you want to Delete this item?');
            if (confirmPopup === true) {

                if (DeleteRecord.amstB_Id > 0) {
                    $scope.deleteId = DeleteRecord.amstB_Id;
                    var MdeleteId = $scope.deleteId;
                    apiService.DeleteURI("StudentAdmission/DeletePrevSchoolEntry", MdeleteId)
                    $scope.$apply();
                    swal("Record Deleted Successfully");

                    $state.reload();
                }
                else {
                    $scope.bondList.splice($scope.bondList.indexOf(DeleteRecord), 1);
                }

            }

        };


        $scope.Deletedata = function (DeleteRecord, SweetAlert) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.deleteId = DeleteRecord.amcsT_Id;
                        var MdeleteId = $scope.deleteId;

                        var data = {
                            "AMCST_Id": MdeleteId
                        };

                        apiService.create("CollegeStudentAdmission/DeleteEntry/", data).then(function (promise) {
                            if (promise.message != "" && promise.message != null) {
                                return swal(promise.message);
                            }
                            else {
                                swal("Failed To Delete Record");
                            }

                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }


        $scope.EditRecord = [];


        $scope.Editdata = function (EditRecord, allRefrence, allSources, allActivity) {

            $scope.disableinstname = true;
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.emails = [{ id: 'email1' }];

            $scope.mobiles1 = [{ id: 'mobile2' }];
            $scope.emails1 = [{ id: 'email2' }];

            $scope.mobilesstd = [{ id: 'mobilesstd' }];
            $scope.emailsstd = [{ id: 'emailsstd' }];

            $scope.EditId = EditRecord.amcsT_Id;
            var MEditId = $scope.EditId;
            $scope.castedisble = false;
            $scope.obj.AMCOC_Id = "";


            apiService.getURI("CollegeStudentAdmission/Edit/", MEditId).
                then(function (promise) {

                    $scope.documentList = promise.documentList;
                    $scope.DOB = false;
                    $scope.mi_id = promise.mI_Id;
                    $scope.address = false;
                    $scope.Parents = false;
                    $scope.Others = false;
                    $scope.DocumentUpload = false;

                    if ($scope.govtBondList != null && promise.bondList != null) {

                        for (var i = 0; i < $scope.govtBondList.length; i++) {
                            $scope.govtBondList[i].Selected = false;
                            $scope.govtBondList[i].amstB_BandNo = "";
                        }
                        for (var i = 0; i < $scope.govtBondList.length; i++) {
                            name = $scope.govtBondList[i].imgB_Id;
                            for (var j = 0; j < promise.bondList.length; j++) {
                                if (name == promise.bondList[j].imgB_Id) {
                                    $scope.govtBondList[i].Selected = true;
                                    $scope.govtBondList[i].amstB_BandNo = promise.bondList[j].amstB_BandNo;

                                }
                            }
                        }
                    }




                    if (promise.prevSchoolDetails != null) {
                        if (promise.prevSchoolDetails.length > 0) {

                            $scope.prevSchoolDetails = promise.prevSchoolDetails;
                            $scope.prevschlcount = promise.prevSchoolDetails.length;

                            for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                                $scope.prevSchoolDetails[i].acstpS_PreSchoolBoard = promise.prevSchoolDetails[i].acstpS_PreSchoolBoard;
                                $scope.prevSchoolDetails[i].acstpS_PreSchoolType = promise.prevSchoolDetails[i].acstpS_PreSchoolType;
                                $scope.prevSchoolDetails[i].acstpS_PreviousClass = promise.prevSchoolDetails[i].acstpS_PreviousClass;
                                $scope.prevSchoolDetails[i].acstpS_PreviousTCNo = promise.prevSchoolDetails[i].acstpS_PreviousTCNo;
                                $scope.prevSchoolDetails[i].acstpS_PreviousRegNo = promise.prevSchoolDetails[i].acstpS_PreviousRegNo;
                                $scope.prevSchoolDetails[i].acstpS_PreviousBranch = promise.prevSchoolDetails[i].acstpS_PreviousBranch;
                                $scope.prevSchoolDetails[i].acstpS_MediumOfInst = promise.prevSchoolDetails[i].acstpS_MediumOfInst;
                                $scope.prevSchoolDetails[i].acstpS_PreviousExamPassed = promise.prevSchoolDetails[i].acstpS_PreviousExamPassed;
                                $scope.prevSchoolDetails[i].acstpS_PasssedMonthYear = promise.prevSchoolDetails[i].acstpS_PasssedMonthYear;
                                $scope.prevSchoolDetails[i].acstpS_LanguagesTaken = promise.prevSchoolDetails[i].acstpS_LanguagesTaken;
                                $scope.prevSchoolDetails[i].acstpS_PreviousTCDate = new Date(promise.prevSchoolDetails[i].acstpS_PreviousTCDate);
                                $scope.prevSchoolDetails[i].acstpS_LeftYear = promise.prevSchoolDetails[i].acstpS_LeftYear;
                                $scope.prevSchoolDetails[i].acstpS_PreSchoolCountry = promise.prevSchoolDetails[i].acstpS_PreSchoolCountry;
                                //  getPrevGetState(promise.prevSchoolDetails[i].acstpS_PreSchoolCountry, promise.prevSchoolDetails[i].acstpS_PreSchoolState);
                                $scope.onselectprevCountry($scope.prevSchoolDetails[i].acstpS_PreSchoolCountry);
                                $scope.prevSchoolDetails[i].acstpS_PreSchoolState = promise.prevSchoolDetails[i].acstpS_PreSchoolState;
                            }
                        }
                    }
                    else {
                        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                        $scope.prevschlcount = 0;
                    }


                    if (promise.adm_College_Student_SubjectMarksDTO != null) {
                        if (promise.adm_College_Student_SubjectMarksDTO.length > 0) {

                            $scope.prevexammarksdetails = promise.adm_College_Student_SubjectMarksDTO;
                            $scope.prevexammarksdetailscount = promise.adm_College_Student_SubjectMarksDTO.length;
                        }
                    }
                    else {
                        $scope.prevexammarksdetails = [{ id: 'prevExamdetails1' }];
                        $scope.prevexammarksdetailscount = 0;
                    }



                    if (promise.studentGuardianDetails != null) {
                        if (promise.studentGuardianDetails.length > 0) {
                            $scope.studentGuardianDetails = promise.studentGuardianDetails;
                            $scope.grddetcount = promise.studentGuardianDetails.length;
                        }
                    }

                    else {
                        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                        $scope.grddetcount = 0;
                    }
                    if (promise.studentSiblingDetails != null) {
                        if (promise.studentSiblingDetails.length > 0) {

                            $scope.studentSiblingDetails = promise.studentSiblingDetails;
                            $scope.sibcount = promise.studentSiblingDetails.length;
                        }
                    }
                    else {
                        $scope.studentSiblingDetails = [{ id: 'sibling1' }];
                        $scope.sibcount = 0;
                    }


                    $scope.studentReferenceDetails = promise.studentReferenceDetails;
                    $scope.studentSourceDetails = promise.studentSourceDetails;
                    $scope.studentActivityDetails = promise.studentActivityDetails;


                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        $scope.allRefrence[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        name = $scope.allRefrence[i].pamR_Id;
                        if (promise.studentReferenceDetails != null) {

                            for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                                if (name == promise.studentReferenceDetails[j].asrR_Id) {
                                    $scope.allRefrence[i].Selected = true;

                                }
                            }
                        }
                    }
                    for (var i = 0; i < $scope.allSources.length; i++) {
                        $scope.allSources[i].Selected = false;
                    }

                    for (var i = 0; i < $scope.allSources.length; i++) {
                        name = $scope.allSources[i].pamS_Id;

                        if (promise.studentSourceDetails != null) {
                            for (var j = 0; j < promise.studentSourceDetails.length; j++) {
                                if (name == promise.studentSourceDetails[j].asrS_Id) {
                                    $scope.allSources[i].Selected = true;

                                }
                            }
                        }
                    }

                    //for (var i = 0; i < $scope.allActivity.length; i++) {
                    //    $scope.allActivity[i].Selected = false;
                    //}
                    //for (var i = 0; i < $scope.allActivity.length; i++) {
                    //    name = $scope.allActivity[i].amA_Id;
                    //    for (var j = 0; j < promise.studentActivityDetails.length; j++) {
                    //        if (name == promise.studentActivityDetails[j].amA_Id) {
                    //            $scope.allActivity[i].Selected = true;

                    //        }
                    //    }
                    //}
                    //documnets
                    if (promise.documentList != null) {
                        if (promise.documentList.length > 0) {
                            $scope.document = {};
                            $scope.documentList = promise.documentList;
                            angular.forEach(promise.documentList, function (value, key) {
                                $('#' + value.amsmD_Id).attr('src', value.document_Path);
                            })
                        }
                    }




                    $('#blah').attr('src', promise.studentList[0].amcsT_StudentPhoto);



                    $scope.fatherphoto = promise.studentList[0].amcsT_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].amcsT_FatherSign;
                    $scope.fatherFingerprint = promise.studentList[0].amcsT_FatherFingerprint;
                    $scope.motherphoto = promise.studentList[0].amcsT_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].amcsT_MotherSign;
                    $scope.motherfingerprint = promise.studentList[0].amcsT_MotherFingerprint;
                    $scope.obj.image = promise.studentList[0].amcsT_StudentPhoto;

                    $scope.obj.AMCST_FirstName = promise.studentList[0].amcsT_FirstName;

                    $scope.obj.AMCST_MiddleName = promise.studentList[0].amcsT_MiddleName;
                    $scope.obj.AMCST_LastName = promise.studentList[0].amcsT_LastName;
                    $scope.obj.AMCST_Date = new Date(promise.studentList[0].amcsT_Date);
                    $scope.obj.AMCST_RegistrationNo = promise.studentList[0].amcsT_RegistrationNo;
                    $scope.obj.AMCST_AdmNo = promise.studentList[0].amcsT_AdmNo;
                    $scope.obj.ASMAY_Id = promise.studentList[0].asmaY_Id;
                    $scope.obj.AMCO_Id = promise.studentList[0].amcO_Id;
                    $scope.obj.AMB_Id = promise.studentList[0].amB_Id;
                    $scope.obj.AMSE_Id = promise.studentList[0].amsE_Id;
                    $scope.obj.ACMB_Id = promise.studentList[0].acmB_Id;
                    $scope.obj.ACQ_Id = promise.studentList[0].acQ_Id;
                    $scope.obj.ACQC_Id = promise.studentList[0].acqC_Id;
                    $scope.obj.ACSS_Id = promise.studentList[0].acsS_Id;
                    $scope.obj.ACST_Id = promise.studentList[0].acsT_Id;
                    if (promise.studentCategory.length > 0) {
                        $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    }
                    $scope.obj.AMCST_Sex = promise.studentList[0].amcsT_Sex;
                    $scope.obj.AMCST_DOB = new Date(promise.studentList[0].amcsT_DOB);
                    $scope.obj.AMCST_DOBin_words = promise.studentList[0].amcsT_DOBin_words;
                    $scope.obj.AMCST_Age = promise.studentList[0].amcsT_Age;
                    $scope.obj.AMCST_BloodGroup = promise.studentList[0].amcsT_BloodGroup;
                    $scope.obj.AMCST_MotherTongue = promise.studentList[0].amcsT_MotherTongue;
                    $scope.obj.AMCST_BirthCertNo = promise.studentList[0].amcsT_BirthCertNo;
                    $scope.obj.IVRMMR_Id = promise.studentList[0].ivrmmR_Id;
                    $scope.obj.AMCST_StudentSubCaste = promise.studentList[0].amcsT_StudentSubCaste;
                    $scope.obj.AMCST_TPINNO = promise.studentList[0].amcsT_TPINNO;

                    $scope.obj.AMCST_Village = promise.studentList[0].amcsT_Village;
                    $scope.obj.AMCST_Taluk = promise.studentList[0].amcsT_Taluk;
                    $scope.obj.AMCST_District = promise.studentList[0].amcsT_District;
                    $scope.obj.AMCST_Urban_Rural = promise.studentList[0].amcsT_Urban_Rural;
                    $scope.obj.AMCST_Divyangjan = promise.studentList[0].amcsT_Divyangjan;

                    $scope.obj.IMCC_Id = promise.studentList[0].imcC_Id;
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        $scope.allCaste[i].Selected = false;
                        $scope.obj.IMC_Id = "";
                    }


                    if (promise.allCaste.length > 0) {
                        for (var i = 0; i < promise.allCaste.length; i++) {
                            if (promise.studentList[0].imC_Id == promise.allCaste[i].imC_Id) {
                                $scope.allCaste[i].Selected = true;
                                $scope.obj.IMC_Id = promise.studentList[0].imC_Id;
                            }
                        }
                    }
                    else {
                        swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    }




                    $scope.obj.AMCST_Nationality = promise.studentList[0].amcsT_Nationality;

                    $scope.obj.IVRMMC_Id = promise.studentList[0].ivrmmC_Id;



                    getSelectGetState($scope.obj.IVRMMC_Id, promise.studentList[0].amcsT_PerState);

                    $scope.obj.AMCST_PerState = promise.studentList[0].amcsT_PerState;


                    $scope.obj.AMCST_PerStreet = promise.studentList[0].amcsT_PerStreet;
                    $scope.obj.AMCST_PerArea = promise.studentList[0].amcsT_PerArea;
                    $scope.obj.AMCST_PerCity = promise.studentList[0].amcsT_PerCity;

                    $scope.obj.AMCST_PerPincode = promise.studentList[0].amcsT_PerPincode;


                    $scope.obj.AMCST_StuBankAccNo = promise.studentList[0].amcsT_StuBankAccNo;
                    $scope.obj.AMCST_StuBankIFSCCode = promise.studentList[0].amcsT_StuBankIFSCCode;
                    $scope.obj.AMCST_AadharNo = promise.studentList[0].amcsT_AadharNo;
                    $scope.obj.AMCST_BirthPlace = promise.studentList[0].amcsT_BirthPlace;
                    $scope.obj.AMCST_StuCasteCertiNo = promise.studentList[0].amcsT_StuCasteCertiNo;
                    $scope.obj.AMCST_MobileNo = promise.studentList[0].amcsT_MobileNo;
                    $scope.obj.AMCST_emailId = promise.studentList[0].amcsT_emailId;

                    $scope.obj.AMCST_PerStreet = promise.studentList[0].amcsT_PerStreet;
                    $scope.obj.AMCST_ConPincode = promise.studentList[0].amcsT_ConPincode;
                    $scope.obj.AMCST_ConArea = promise.studentList[0].amcsT_ConArea;
                    $scope.obj.AMCST_ConStreet = promise.studentList[0].amcsT_ConStreet;
                    $scope.obj.AMCST_ConCity = promise.studentList[0].amcsT_ConCity;
                    $scope.obj.AMCST_ConCountryId = promise.studentList[0].amcsT_ConCountryId;

                    getSelectGetState2($scope.obj.AMCST_ConCountryId, promise.studentList[0].amcsT_ConState);

                    $scope.obj.AMCST_ConState = promise.studentList[0].amcsT_ConState;

                    $scope.obj.AMCST_FatherAliveFlag = promise.studentList[0].amcsT_FatherAliveFlag;

                    $scope.obj.AMCST_FatherName = promise.studentList[0].amcsT_FatherName;
                    $scope.obj.AMCST_FatherSurname = promise.studentList[0].amcsT_FatherSurname;
                    $scope.obj.AMCST_FatherAadharNo = promise.studentList[0].amcsT_FatherAadharNo;
                    $scope.obj.AMCST_FatherEducation = promise.studentList[0].amcsT_FatherEducation;
                    $scope.obj.AMCST_FatherOfficeAdd = promise.studentList[0].amcsT_FatherOfficeAdd;
                    $scope.obj.AMCST_FatherOccupation = promise.studentList[0].amcsT_FatherOccupation;
                    $scope.obj.AMCST_FatherDesignation = promise.studentList[0].amcsT_FatherDesignation;
                    $scope.obj.AMCST_FatherBankAccNo = promise.studentList[0].amcsT_FatherBankAccNo;
                    $scope.obj.AMCST_FatherBankIFSCCode = promise.studentList[0].amcsT_FatherBankIFSCCode;
                    $scope.obj.AMCST_FatherCasteCertiNo = promise.studentList[0].amcsT_FatherCasteCertiNo;
                    $scope.obj.AMCST_FatherNationality = promise.studentList[0].amcsT_FatherNationality;
                    $scope.obj.AMCST_FatherMonIncome = promise.studentList[0].amcsT_FatherMonIncome;
                    $scope.obj.AMCST_FatherAnnIncome = promise.studentList[0].amcsT_FatherAnnIncome;
                    $scope.obj.AMCST_FatherMobleNo = promise.studentList[0].amcsT_FatherMobleNo;
                    $scope.obj.AMCST_FatheremailId = promise.studentList[0].amcsT_FatheremailId;
                    $scope.obj.AMCST_FatherReligion = promise.studentList[0].amcsT_FatherReligion;
                    $scope.obj.AMCST_FatherCaste = promise.studentList[0].amcsT_FatherCaste;
                    $scope.obj.AMCST_FatherSubCaste = promise.studentList[0].amcsT_FatherSubCaste;

                    $scope.obj.AMCST_MotherAliveFlag = promise.studentList[0].amcsT_MotherAliveFlag;

                    $scope.obj.AMCST_MotherName = promise.studentList[0].amcsT_MotherName;
                    $scope.obj.AMCST_MotherSurname = promise.studentList[0].amcsT_MotherSurname;
                    $scope.obj.AMCST_MotherAadharNo = promise.studentList[0].amcsT_MotherAadharNo;
                    $scope.obj.AMCST_MotherEducation = promise.studentList[0].amcsT_MotherEducation;
                    $scope.obj.AMCST_MotherOfficeAdd = promise.studentList[0].amcsT_MotherOfficeAdd;
                    $scope.obj.AMCST_MotherOccupation = promise.studentList[0].amcsT_MotherOccupation;
                    $scope.obj.AMCST_MotherDesignation = promise.studentList[0].amcsT_MotherDesignation;
                    $scope.obj.AMCST_MotherBankAccNo = promise.studentList[0].amcsT_MotherBankAccNo;
                    $scope.obj.AMCST_MotherBankIFSCCode = promise.studentList[0].amcsT_MotherBankIFSCCode;
                    $scope.obj.AMCST_MotherCasteCertiNo = promise.studentList[0].amcsT_MotherCasteCertiNo;
                    $scope.obj.AMCST_MotherNationality = promise.studentList[0].amcsT_MotherNationality;
                    $scope.obj.AMCST_MotherMonIncome = promise.studentList[0].amcsT_MotherMonIncome;
                    $scope.obj.AMCST_MotherAnnIncome = promise.studentList[0].amcsT_MotherAnnIncome;
                    $scope.obj.AMCST_MotherMobleNo = promise.studentList[0].amcsT_MotherMobleNo;
                    $scope.obj.AMCST_MotheremailId = promise.studentList[0].amcsT_MotheremailId;

                    $scope.obj.AMCST_MotherReligion = promise.studentList[0].amcsT_MotherReligion;
                    $scope.obj.AMCST_MotherCaste = promise.studentList[0].amcsT_MotherCaste;
                    $scope.obj.AMCST_MotherSubCaste = promise.studentList[0].amcsT_MotherSubCaste;
                    // $scope.BindData();

                    //if (promise.studentAchivementDetails.length > 0) {
                    //    $scope.obj.amsteC_Extracurricular = promise.studentAchivementDetails[0].amsteC_Extracurricular;
                    //}
                    //else {
                    //    $scope.obj.amsteC_Extracurricular = "";
                    //}

                    $scope.obj.AMCST_BPLCardFlag = promise.studentList[0].amcsT_BPLCardFlag;
                    //if (promise.studentList[0].amcsT_BPLCardFlag == true) {

                    //    $scope.obj.amsT_BPLCardFlag = true;
                    //}
                    //else {
                    //    $scope.obj.amsT_BPLCardFlag = false;
                    //}

                    $scope.obj.AMCST_BPLCardNo = promise.studentList[0].amcsT_BPLCardNo;
                    $scope.obj.AMCST_HostelReqdFlag = promise.studentList[0].amcsT_HostelReqdFlag;
                    //if ($scope.obj.amsT_HostelReqdFlag === 1) {
                    //    $scope.obj.amsT_HostelReqdFlag = true;
                    //}
                    //else {
                    //    $scope.obj.amsT_HostelReqdFlag = false;
                    //}
                    $scope.obj.AMCST_TransportReqdFlag = promise.studentList[0].amcsT_TransportReqdFlag;
                    //if ($scope.obj.amsT_TransportReqdFlag === 1) {
                    //    $scope.obj.amsT_TransportReqdFlag = true;
                    //}
                    //else {
                    //    $scope.obj.amsT_TransportReqdFlag = false;
                    //}
                    $scope.obj.AMCST_GymReqdFlag = promise.studentList[0].amcsT_GymReqdFlag;
                    //if ($scope.obj.amsT_GymReqdFlag === 1) {
                    //    $scope.obj.amsT_GymReqdFlag = true;
                    //}
                    //else {
                    //    $scope.obj.amsT_GymReqdFlag = false;
                    //}
                    $scope.obj.AMCST_ECSFlag = promise.studentList[0].amcsT_ECSFlag;
                    //if ($scope.obj.amsT_ECSFlag === 1) {
                    //    $scope.obj.amsT_ECSFlag = true;
                    //}
                    //else {
                    //    $scope.obj.amsT_ECSFlag = false;
                    //}
                    $scope.obj.chkbox_address = 0;
                    if ($scope.obj.AMCST_PerStreet == $scope.obj.AMCST_ConStreet && $scope.obj.AMCST_PerArea == $scope.obj.AMCST_ConArea && $scope.obj.IVRMMC_Id == $scope.obj.AMCST_ConCountryId && $scope.obj.AMCST_PerState == $scope.obj.AMCST_ConState && $scope.obj.AMCST_PerCity == $scope.obj.AMCST_ConCity && $scope.obj.AMCST_PerPincode == $scope.obj.AMCST_ConPincode) {
                        $scope.obj.chkbox_address = 1;
                    }
                    $scope.obj.feeconcession = promise.studentList[0].amsT_Concession_Type;
                    if (promise.classsection != null && promise.classsection != "") {
                        $scope.lblclasssection = promise.classsection[0].classname + '-' + promise.classsection[0].sectionname;
                    }
                    else {
                        $scope.lblclasssection = "N/A";
                    }
                    // father mobile no
                    if (promise.fatherMultipleMobileNoDTO.length > 0) {
                        $scope.mobiles = promise.fatherMultipleMobileNoDTO;
                    }
                    //father email no
                    if (promise.fatherMultipleEmailIdDTO.length > 0) {
                        $scope.emails = promise.fatherMultipleEmailIdDTO;
                    }
                    //mother mobileno
                    if (promise.motherMultipleMobileNoDTO.length > 0) {
                        $scope.mobiles1 = promise.motherMultipleMobileNoDTO;
                    }
                    //mother email
                    if (promise.motherMultipleEmailIdDTO.length > 0) {
                        $scope.emails1 = promise.motherMultipleEmailIdDTO;
                    }


                    //student mobileno
                    if (promise.adm_College_Student_SMSNoDTO.length > 0) {
                        $scope.mobilesstd = promise.adm_College_Student_SMSNoDTO;
                    }
                    //student email
                    if (promise.adm_College_Student_EmailIdDTO.length > 0) {
                        $scope.emailsstd = promise.adm_College_Student_EmailIdDTO;
                    }


                    $scope.obj.AMCST_PassportNo = promise.studentList[0].amcsT_PassportNo;
                    if ($scope.obj.AMCST_PassportNo != "" && $scope.obj.AMCST_PassportNo != null) {
                        $scope.obj.AMCST_PassportIssuedAt = promise.studentList[0].amcsT_PassportIssuedAt;
                        $scope.obj.AMCST_PassportIssueDate = new Date(promise.studentList[0].amcsT_PassportIssueDate);
                        $scope.obj.AMCST_PassportIssuedCounrty = promise.studentList[0].amcsT_PassportIssuedCounrty;
                        $scope.obj.AMCST_PassportIssuedPlace = promise.studentList[0].amcsT_PassportIssuedPlace;
                        $scope.obj.AMCST_PassportExpiryDate = new Date(promise.studentList[0].amcsT_PassportExpiryDate);
                    }
                    $scope.obj.AMCST_VisaIssuedBy = promise.studentList[0].amcsT_VisaIssuedBy;
                    if ($scope.obj.AMCST_VisaIssuedBy != "" && $scope.obj.AMCST_VisaIssuedBy != null) {

                        $scope.obj.AMCST_VISAValidFrom = new Date(promise.studentList[0].amcsT_VISAValidFrom);
                        $scope.obj.AMCST_VISAValidTo = new Date(promise.studentList[0].amcsT_VISAValidTo);
                    }






                    //   $scope.onclasschange($scope.obj.asmcL_Id, $scope.obj.asmaY_Id, 1);


                    //if (promise.adm_M_Student_ECSDTo.length > 0) {
                    //    $scope.ecsdetailslist = promise.adm_M_Student_ECSDTo[0];
                    //    $scope.objecs = {};
                    //    $scope.tempecs = promise.adm_M_Student_ECSDTo;

                    //    $scope.objecs.asecS_AccountNo = $scope.tempecs[0].asecS_AccountNo;
                    //    $scope.objecs.asecS_AccountHolderName = $scope.tempecs[0].asecS_AccountHolderName;

                    //    $scope.objecs.asecS_BankName = $scope.tempecs[0].asecS_BankName;
                    //    $scope.objecs.asecS_AccountType = $scope.tempecs[0].asecS_AccountType;

                    //    $scope.objecs.asecS_MICRNo = $scope.tempecs[0].asecS_MICRNo;
                    //    $scope.objecs.asecS_Branch = $scope.tempecs[0].asecS_Branch;

                    //    $scope.objecs.asecS_Id = $scope.tempecs[0].asecS_Id;

                    //}

                })
        };


        //get permenent address state while editing
        function getSelectGetState(countryidd, stateid) {
            apiService.getURI("CollegeStudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.AMCST_PerState = sts;
                $scope.data = promise.allState;
                $scope.allState.push.apply($scope.allState, $scope.data);
                $scope.statelabel = false;
            })
        }
        //get Previous School State while editing.
        function getPrevGetState(countryidd, stateid) {
            var data = {
                countryName: countryidd
            }

            apiService.create("CollegeStudentAdmission/StateByCountryName", data).then(function (promise) {
                $scope.prevState = [{ "ivrmmS_Name": "", "ivrmmS_Name": "--Select--" }];
                var sts = stateid;
                $scope.prevSchool.acstpS_PreSchoolState = sts;
                $scope.data = promise.prevStateList;
                $scope.prevState.push.apply($scope.prevState, $scope.data);
            })
        }
        //get Residential address state while editing
        function getSelectGetState2(countryidd, stateid) {
            apiService.getURI("CollegeStudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.AMCST_ConState = sts;
                $scope.data2 = promise.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;
            })
        }






        $scope.Activitycheckboxchcked = [];

        $scope.CheckedActivityName = function (data) {

            if ($scope.Activitycheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allActivity.length; i++) {
                    if ($scope.allActivity[i].Selected == true) {
                        $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                    }
                }
            }
            else {
                $scope.Activitycheckboxchcked.splice($scope.Activitycheckboxchcked.indexOf(data), 1);
            }
        }





        $scope.Refrencecheckboxchcked = [];

        $scope.CheckedRefrenceName = function (data) {

            if ($scope.Refrencecheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    if ($scope.allRefrence[i].Selected == true) {
                        $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                    }
                }
            }
            else {
                $scope.Refrencecheckboxchcked.splice($scope.Refrencecheckboxchcked.indexOf(data), 1);
            }
        }





        $scope.Sourcescheckboxchcked = [];

        $scope.CheckedSourcesName = function (data) {

            if ($scope.Sourcescheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allSources.length; i++) {
                    if ($scope.allSources[i].Selected == true) {
                        $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                    }
                }
            }
            else {
                $scope.Sourcescheckboxchcked.splice($scope.Sourcescheckboxchcked.indexOf(data), 1);
            }
        }

        //Get Satate by country
        $scope.onSelectGetState = function (IVRMMC_Id) {
            var countryidd = IVRMMC_Id;
            apiService.getURI("CollegeStudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState = promise.allState;
                $scope.obj.AMCST_PerState = promise.allState[0].amcsT_PerState;
                $scope.statelabel = true;
            })
        }

        //get the state name by country  pre school
        $scope.onSelectGetStatepre = function (IVRMMC_Id5) {

            var countryidd = IVRMMC_Id5;
            apiService.getURI("CollegeStudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState12 = promise.allState;
                $scope.statelabel = true;
            })
        }

        // Get city by state
        $scope.onSelectGetCity = function (ivrmmS_Id) {
            var stateId = ivrmmS_Id;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity = promise.allCity;
            })
        }

        //Get Satate by country
        $scope.onSelectGetState1 = function (IVRMMC_Id2) {
            var countryidd = IVRMMC_Id2;
            apiService.getURI("CollegeStudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = promise.allState;
                $scope.obj.AMCST_ConState = promise.allState[0].amcsT_ConState;
                $scope.statelabel2 = true;
            })
        }

        // Get city by state
        $scope.onSelectGetCity1 = function (ivrmmS_Id2) {
            var stateId = ivrmmS_Id2;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity1 = promise.allCity;
            })
        }



        $scope.address_copy = function () {

            if ($scope.obj.chkbox_address == 1) {
                $scope.obj.AMCST_ConStreet = $scope.obj.AMCST_PerStreet;
                $scope.obj.AMCST_ConArea = $scope.obj.AMCST_PerArea;
                $scope.obj.AMCST_ConCountryId = $scope.obj.IVRMMC_Id;

                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number($scope.obj.AMCST_PerState);
                $scope.obj.AMCST_ConState = sts;

                $scope.data2 = $scope.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;

                $scope.obj.AMCST_ConCity = $scope.obj.AMCST_PerCity;
                $scope.obj.AMCST_ConPincode = $scope.obj.AMCST_PerPincode;

            }
            if ($scope.obj.chkbox_address == 0) {
                $scope.obj.AMCST_ConStreet = "";
                $scope.obj.AMCST_ConArea = "";
                $scope.obj.AMCST_ConCountryId = "";
                $scope.obj.AMCST_ConState = "";
                $scope.obj.AMCST_ConCity = "";
                $scope.obj.AMCST_ConPincode = "";

            }

        }

        $scope.SelectedGovtBonds = [];


        //------------------save method---------------------------------//
        $scope.savedata = function () {


            if ($scope.myForm1.$valid && $scope.myForm2.$valid && $scope.myForm3.$valid && $scope.myForm4.$valid && $scope.myForm5.$valid) {

                $scope.disableSaveButton = true;

                if ($scope.allActivity != "" && $scope.allActivity != null) {
                    if ($scope.allActivity.length > 0) {
                        for (var i = 0; i < $scope.allActivity.length; i++) {
                            if ($scope.allActivity[i].Selected == true) {
                                $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                            }
                        }
                    }
                }

                if ($scope.allRefrence != "" && $scope.allRefrence != null) {
                    if ($scope.allRefrence.length > 0) {
                        for (var i = 0; i < $scope.allRefrence.length; i++) {
                            if ($scope.allRefrence[i].Selected == true) {
                                $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                            }
                        }
                    }
                }
                if ($scope.allSources != "" && $scope.allSources != null) {
                    if ($scope.allSources.length > 0) {
                        for (var i = 0; i < $scope.allSources.length; i++) {
                            if ($scope.allSources[i].Selected == true) {
                                $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                            }
                        }
                    }
                }

                if ($scope.govtBondList != "" && $scope.govtBondList != null) {
                    if ($scope.govtBondList.length > 0) {
                        for (var i = 0; i < $scope.govtBondList.length; i++) {
                            if ($scope.govtBondList[i].Selected == true) {
                                $scope.SelectedGovtBonds.push($scope.govtBondList[i]);
                            }
                        }
                    }
                }



                var ActivityIDs = $scope.Activitycheckboxchcked;
                var RefrenceIds = $scope.Refrencecheckboxchcked;
                var SourcesIds = $scope.Sourcescheckboxchcked;

                if ($scope.prevSchoolDetails.length > 0) {
                    angular.forEach($scope.prevSchoolDetails, function (pr) {
                        pr.acstpS_PreviousTCDate = new Date(pr.acstpS_PreviousTCDate).toDateString();
                    })
                }

                var PrevSchoolDet = $scope.prevSchoolDetails;

                var StuGuardianDet = $scope.studentGuardianDetails;
                var StuSiblingDetails = $scope.studentSiblingDetails;

                if ($scope.obj.amsT_FatherAliveFlag === true) {
                    $scope.obj.amsT_FatherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_FatherAliveFlag = "false";
                }

                if ($scope.obj.amsT_MotherAliveFlag === true) {
                    $scope.obj.amsT_MotherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_MotherAliveFlag = "false";
                }

                if ($scope.obj.amsT_BPLCardFlag === true) {
                    $scope.obj.amsT_BPLCardFlag = 1;
                }
                else {
                    $scope.obj.amsT_BPLCardFlag = 0;
                }

                if ($scope.obj.amsT_HostelReqdFlag === true) {
                    $scope.obj.amsT_HostelReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_HostelReqdFlag = 0;
                }

                if ($scope.obj.amsT_TransportReqdFlag === true) {
                    $scope.obj.amsT_TransportReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_TransportReqdFlag = 0;
                }

                if ($scope.obj.amsT_GymReqdFlag === true) {
                    $scope.obj.amsT_GymReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_GymReqdFlag = 0;
                }

                if ($scope.obj.amsT_ECSFlag === true) {
                    $scope.obj.amsT_ECSFlag = 1;
                }
                else {
                    $scope.obj.amsT_ECSFlag = 0;
                }

                var amsT_Date = new Date($scope.amsT_Date).toDateString();
                var amsT_DOB = new Date($scope.obj.amsT_DOB).toDateString();

                var fathermobileno = $scope.mobiles;
                var fatheremailid = $scope.emails;
                var mothermobileno = $scope.mobiles1;
                var motheremailid = $scope.emails1;

                if ($scope.obj.amsT_FirstName == null) {
                    $scope.obj.amsT_FirstName = '';
                } else {
                    $scope.obj.amsT_FirstName = $scope.obj.amsT_FirstName;
                }

                if ($scope.obj.amsT_MiddleName == null) {
                    $scope.obj.amsT_MiddleName = '';
                } else {
                    $scope.obj.amsT_MiddleName = $scope.obj.amsT_MiddleName;
                }

                if ($scope.obj.amsT_LastName == null) {
                    $scope.obj.amsT_LastName = '';
                } else {
                    $scope.obj.amsT_LastName = $scope.obj.amsT_LastName;
                }

                //Student Sub caste
                if ($scope.obj.amsT_SubCasteIMC_Id == null) {
                    $scope.obj.amsT_SubCasteIMC_Id = '';
                }
                else {
                    $scope.obj.amsT_SubCasteIMC_Id = $scope.obj.amsT_SubCasteIMC_Id;
                }

                //Father Sub caste
                if ($scope.obj.amsT_FatherSubCaste == null) {
                    $scope.obj.amsT_FatherSubCaste = '';
                }
                else {
                    $scope.obj.amsT_FatherSubCaste = $scope.obj.amsT_FatherSubCaste;
                }

                //Mother Sub caste
                if ($scope.obj.amsT_MotherSubCaste == null) {
                    $scope.obj.amsT_MotherSubCaste = '';
                }
                else {
                    $scope.obj.amsT_MotherSubCaste = $scope.obj.amsT_MotherSubCaste;
                }






                var data = {
                    "AMST_Id": $scope.EditId,
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "AMST_FirstName": $scope.obj.amsT_FirstName,
                    "AMST_MiddleName": $scope.obj.amsT_MiddleName,
                    "AMST_LastName": $scope.obj.amsT_LastName,
                    "AMST_Date": amsT_Date,
                    "AMST_RegistrationNo": $scope.obj.amsT_RegistrationNo,
                    "AMST_AdmNo": $scope.obj.amsT_AdmNo,
                    "AMC_Id": $scope.obj.stucategory,
                    "AMST_Sex": $scope.amsT_Sex,
                    "AMST_DOB": amsT_DOB,
                    "AMST_DOB_Words": $scope.obj.amsT_DOB_Words,
                    "PASR_Age": $scope.obj.pasR_Age,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "AMST_BloodGroup": $scope.obj.amsT_BloodGroup,
                    "AMST_MotherTongue": $scope.obj.amsT_MotherTongue,
                    "AMST_BirthCertNO": $scope.obj.amsT_BirthCertNO,
                    "IVRMMR_Id": $scope.obj.ivrmmR_Id,
                    "IMCC_Id": $scope.obj.imcC_Id,
                    "IMC_Id": $scope.obj.iC_Id,
                    "AMST_PerCountry": $scope.obj.IVRMMC_Id,
                    "AMST_PerStreet": $scope.obj.amsT_PerStreet,
                    "AMST_PerArea": $scope.obj.amsT_PerArea,
                    "AMST_PerCity": $scope.obj.amsT_PerCity,
                    "AMST_PerState": $scope.obj.amsT_PerState,
                    "AMST_PerPincode": $scope.obj.amsT_PerPincode,
                    "AMST_ConStreet": $scope.obj.amsT_ConStreet,
                    "AMST_ConArea": $scope.obj.amsT_ConArea,
                    "AMST_ConCity": $scope.obj.amsT_ConCity,
                    "AMST_ConState": $scope.obj.amsT_ConState,
                    "AMST_ConCountry": $scope.obj.amsT_ConCountry,
                    "AMST_ConPincode": $scope.obj.amsT_ConPincode,
                    "AMST_AadharNo": $scope.obj.amsT_AadharNo,
                    "AMST_StuBankAccNo": $scope.obj.amsT_StuBankAccNo,
                    "AMST_StuBankIFSC_Code": $scope.obj.amsT_StuBankIFSC_Code,
                    "AMST_StuCasteCertiNo": $scope.obj.amsT_StuCasteCertiNo,
                    "AMST_MobileNo": $scope.obj.amsT_MobileNo,
                    "AMST_emailId": $scope.obj.amsT_emailId,
                    "AMST_Photoname": $scope.obj.image,
                    "AMST_BirthPlace": $scope.obj.amsT_BirthPlace,
                    "AMST_FatherAliveFlag": $scope.obj.amsT_FatherAliveFlag,
                    "AMST_FatherName": $scope.obj.amsT_FatherName,
                    "AMST_FatherAadharNo": $scope.obj.amsT_FatherAadharNo,
                    "AMST_FatherSurname": $scope.obj.amsT_FatherSurname,
                    "AMST_FatherEducation": $scope.obj.amsT_FatherEducation,
                    "AMST_FatherOccupation": $scope.obj.amsT_FatherOccupation,
                    "AMST_FatherOfficeAdd": $scope.obj.amsT_FatherOfficeAdd,
                    "AMST_FatherDesignation": $scope.obj.amsT_FatherDesignation,
                    "AMST_FatherMonIncome": $scope.obj.amsT_FatherMonIncome,
                    "AMST_FatherAnnIncome": $scope.obj.amsT_FatherAnnIncome,
                    "AMST_FatherNationality": $scope.obj.IVRMMC_Id3,
                    "AMST_FatherMobleNo": $scope.obj.amsT_FatherMobleNo,
                    "AMST_FatheremailId": $scope.obj.amsT_FatheremailId,
                    "AMST_FatherBankAccNo": $scope.obj.amsT_FatherBankAccNo,
                    "AMST_FatherBankIFSC_Code": $scope.obj.amsT_FatherBankIFSC_Code,
                    "AMST_FatherCasteCertiNo": $scope.obj.amsT_FatherCasteCertiNo,
                    "ANST_FatherPhoto": $scope.fatherphoto,
                    "AMST_MotherAliveFlag": $scope.obj.amsT_MotherAliveFlag,
                    "AMST_MotherName": $scope.obj.amsT_MotherName,
                    "ANST_MotherPhoto": $scope.motherphoto,
                    "AMST_MotherAadharNo": $scope.obj.amsT_MotherAadharNo,
                    "AMST_MotherSurname": $scope.obj.amsT_MotherSurname,
                    "AMST_MotherEducation": $scope.obj.amsT_MotherEducation,
                    "AMST_MotherOccupation": $scope.obj.amsT_MotherOccupation,
                    "AMST_MotherOfficeAdd": $scope.obj.amsT_MotherOfficeAdd,
                    "AMST_MotherDesignation": $scope.obj.amsT_MotherDesignation,
                    "AMST_MotherMonIncome": $scope.obj.amsT_MotherMonIncome,
                    "AMST_MotherAnnIncome": $scope.obj.amsT_MotherAnnIncome,
                    "AMST_MotherNationality": $scope.obj.IVRMMC_Id4,
                    "AMST_MotherMobileNo": $scope.obj.amsT_MotherMobileNo,
                    "AMST_MotherEmailId": $scope.obj.amsT_MotherEmailId,
                    "AMST_MotherBankAccNo": $scope.obj.amsT_MotherBankAccNo,
                    "AMST_MotherBankIFSC_Code": $scope.obj.amsT_MotherBankIFSC_Code,
                    "AMST_MotherCasteCertiNo": $scope.obj.amsT_MotherCasteCertiNo,
                    "AMST_Nationality": $scope.obj.IVRMMC_Id,
                    "AMST_BPLCardFlag": $scope.obj.amsT_BPLCardFlag,
                    "AMST_BPLCardNo": $scope.obj.amsT_BPLCardNo,
                    "AMST_HostelReqdFlag": $scope.obj.amsT_HostelReqdFlag,
                    "AMST_TransportReqdFlag": $scope.obj.amsT_TransportReqdFlag,
                    "AMST_GymReqdFlag": $scope.obj.amsT_GymReqdFlag,
                    "AMST_ECSFlag": $scope.obj.amsT_ECSFlag,
                    "SelectedActivityDetails": ActivityIDs,
                    "SelectedRefrenceDetails": RefrenceIds,
                    "SelectedSourceDetails": SourcesIds,
                    "SelectedAchivementDetails": $scope.obj.amsteC_Extracurricular,
                    "SelectedBondDetails": $scope.SelectedGovtBonds,
                    "SelectedPrevSchoolDetails": PrevSchoolDet,
                    "SelectedSiblingDetails": StuSiblingDetails,
                    "SelectedGuardianDetails": StuGuardianDet,
                    "transnumconfigsettings": $scope.RegistrationNumbering,
                    "admissionNumbering": $scope.AdmissionNumbering,
                    "AMST_Father_Signature": $scope.fatherSign,
                    "AMST_Father_FingerPrint": $scope.fatherFingerprint,
                    "AMST_Mother_Signature": $scope.mothersign,
                    "AMST_Mother_FingerPrint": $scope.motherfingerprint,
                    "AMST_Concession_Type": $scope.obj.feeconcession,
                    "multiplemobileno": fathermobileno,
                    "multipleemailid": fatheremailid,
                    "multiplemobilenomother": mothermobileno,
                    "multipleemailidmother": motheremailid,
                    "Adm_M_Student_MobileNoDTO": $scope.mobilesstd,
                    "Adm_M_Student_EmailIdDTO": $scope.emailsstd,
                    "AMST_SubCasteIMC_Id": $scope.obj.amsT_SubCasteIMC_Id,
                    "AMST_MotherSubCaste": $scope.obj.amsT_MotherSubCaste,
                    "AMST_MotherCaste": $scope.obj.amsT_MotherCaste,
                    "AMST_MotherReligion": $scope.obj.amsT_MotherReligion,
                    "AMST_FatherSubCaste": $scope.obj.amsT_FatherSubCaste,
                    "AMST_FatherCaste": $scope.obj.amsT_FatherCaste,
                    "AMST_FatherReligion": $scope.obj.amsT_FatherReligion,
                    Uploaded_documentList: $scope.documentList,
                    guardianPhoto: $scope.studentGuardianDet,
                    guardianSign: $scope.studentGuardianDet,
                    guardianFingerprint: $scope.studentGuardianDet
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    },
                }
                apiService.create("StudentAdmission/", data).then(function (promise) {


                    if (promise.returnval == true && promise.message == "saved") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == true && promise.message == "updated") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal("Failed to Save/Update");

                    }

                })


            } else {
                $scope.submitted = true;
                $scope.submitted1 = true;
                $scope.submitted2 = true;
                $scope.submitted3 = true;
                $scope.submitted4 = true;
                $scope.disableSaveButton = false;
            }

        };

        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#' + document.amsmD_Id)
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        $scope.UploadStudentProfilePic = [];

        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.UploadStudentProfilePic = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadGuardianPhoto = [];

        $scope.uploadGuardianPhoto = function (input, document) {

            $scope.UploadGuardianPhoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianPhoto.length; i++) {
                formData.append("File", $scope.UploadGuardianPhoto[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.acstG_GuardianPhoto = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        $scope.UploadGuardianSign = [];

        $scope.uploadGuardianSign = function (input, document) {

            $scope.UploadGuardianSign = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {


                    UploaddianSign(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploaddianSign(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianSign.length; i++) {
                formData.append("File", $scope.UploadGuardianSign[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.acstG_GuardianSign = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        $scope.UploadGuardianFingerprint = [];

        $scope.uploadGuardianFingerprint = function (input, document) {

            $scope.UploadGuardianFingerprint = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {


                    UploaddianFingerprint(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }



        function UploaddianFingerprint(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianFingerprint.length; i++) {
                formData.append("File", $scope.UploadGuardianFingerprint[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.acstG_Fingerprint = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }
        $scope.UploadFatherPhoto = [];

        $scope.uploadFatherPhoto = function (input) {

            $scope.UploadFatherPhoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherProfile();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploadFatherProfile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherPhoto.length; i++) {
                formData.append("File", $scope.UploadFatherPhoto[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadFatherSignature = [];

        $scope.uploadFatherSignature = function (input) {

            $scope.UploadFatherSignature = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFathersign();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadFathersign() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherSignature.length; i++) {
                formData.append("File", $scope.UploadFatherSignature[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherSign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadFatherFingerprints = [];

        $scope.uploadFatherFingerprints = function (input) {

            $scope.UploadFatherFingerprints = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherFingerprnts();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadFatherFingerprnts() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherFingerprints.length; i++) {
                formData.append("File", $scope.UploadFatherFingerprints[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherFingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherphoto = [];

        $scope.uploadMotherphoto = function (input) {

            $scope.UploadMotherphoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherProfilepic();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMotherProfilepic() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherphoto.length; i++) {
                formData.append("File", $scope.UploadMotherphoto[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.motherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherSign = [];

        $scope.uploadMotherSign = function (input) {

            $scope.UploadMotherSign = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMothersignature();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMothersignature() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherSign.length; i++) {
                formData.append("File", $scope.UploadMotherSign[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.mothersign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherFingerprints = [];

        $scope.uploadMotherFingerprints = function (input) {

            $scope.UploadMotherFingerprints = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherFing();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMotherFing() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherFingerprints.length; i++) {
                formData.append("File", $scope.UploadMotherFingerprints[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.motherfingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }











        $scope.reload = function () {
            $state.reload();
        }


        //priview document
        $scope.showmodaldetails = function (data) {

            $('#preview').attr('src', data.document_Path);
        }

        $scope.showGuardianPhoto = function (data) {

            $('#preview').attr('src', data.amstG_GuardianPhoto);
        }

        $scope.showGuardianSign = function (data) {

            $('#preview').attr('src', data.amstG_GuardianSign);
        }

        $scope.showGuardianFingerprint = function (data) {

            $('#preview').attr('src', data.amstG_Fingerprint);
        }

        $scope.showfatherPhoto = function (fatherphoto) {
            $('#preview').attr('src', fatherphoto);
        }

        $scope.showfathersign = function (fatherSign) {

            $('#preview').attr('src', fatherSign);
        }

        $scope.showfatherfingerprint = function (fatherFingerprint) {

            $('#preview').attr('src', fatherFingerprint);
        }

        $scope.showmotherprofilepic = function (motherphoto) {

            $('#preview').attr('src', motherphoto);
        }

        $scope.showmothersign = function (mothersign) {

            $('#preview').attr('src', mothersign);
        }

        $scope.showmotherfingerprint = function (motherfingerprint) {
            $('#preview').attr('src', motherfingerprint);
        }






        //Clear functionality
        $scope.clear_first_tab = function (data) {


            $scope.obj.amsT_StuBankAccNo = "";
            $scope.lblclasssection = "";

            $scope.obj.amsT_BPLCardFlag = "";
            if ($scope.EditId > 0) {

            }
            else {
                $scope.obj.stucategory = "";
                $scope.obj.asmcL_Id = "";
                $scope.obj.pasR_Age = "";
                $scope.obj.amsT_BPLCardNo = "";
            }

            $scope.obj.amsT_BloodGroup = "";
            $scope.obj.amsT_AadharNo = "";
            $scope.obj.amsT_emailId = "";
            $scope.obj.amsT_MobileNo = "";
            $scope.obj.IVRMMC_Id = "";
            $scope.obj.image = "";
            $scope.obj.ivrmmR_Id = "";
            $scope.obj.amsT_MotherTongue = "";
            $scope.amsT_Sex = "";
            $scope.obj.amsT_Tpin = "";
            $scope.obj.amsT_SubCasteIMC_Id = "";
            $scope.obj.amsT_BirthCertNO = "";
            $scope.obj.amsT_BirthPlace = "";
            $scope.obj.amsT_StuBankIFSC_Code = "";
            $scope.obj.iC_Id = "";
            $scope.obj.imcC_Id = "";
            $scope.obj.amsT_DOB_Words = "";
            $scope.obj.amsT_DOB = "";

            $scope.amcsT_emailId = "";
            $scope.emailsstd = [];
            $scope.emailstd = {};
            $scope.emailsstd = [{ id: 'emailsstd' }];
            $scope.emailsstd[0].amcsT_emailId = "";

            $scope.mobilesstd = {};
            $scope.mobilesstd = [{ id: 'mobilesstd' }];
            $scope.mobilesstd[0].amcsT_MobileNo = "";

            $scope.obj.amsT_AdmNo = "";
            $scope.obj.amsT_RegistrationNo = "";

            $scope.obj.amsT_LastName = "";
            $scope.obj.amsT_MiddleName = "";
            $scope.obj.amsT_FirstName = "";
            $scope.obj.amsT_StuCasteCertiNo = "";
            $scope.search = "";
            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });
            $scope.obj.image = "";
            $('#blah').removeAttr('src');
            $scope.obj.feeconcession = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();

        }

        $scope.clear_second_tab = function (data) {
            //  $state.reload();

            $scope.obj.amsT_PerStreet = "";
            $scope.obj.amsT_PerArea = "";
            $scope.obj.IVRMMC_Id5 = "";
            $scope.obj.amsT_PerState = "";
            $scope.obj.amsT_PerCity = "";
            $scope.obj.amsT_PerPincode = "";
            $scope.obj.amsT_ConStreet = "";
            $scope.obj.amsT_ConArea = "";
            $scope.obj.amsT_ConCountry = "";
            $scope.obj.amsT_ConState = "";
            $scope.obj.amsT_ConCity = "";
            $scope.obj.amsT_ConPincode = "";
            $scope.search = "";
            $scope.obj.chkbox_address = false;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();


        }

        $scope.clear_third_tab = function (data) {
            $scope.obj.amsT_FatherAliveFlag = "";
            $scope.obj.amsT_FatherName = "";
            $scope.obj.amsT_FatherSurname = "";
            $scope.obj.amsT_FatherAadharNo = "";
            $scope.obj.amsT_FatherEducation = "";
            $scope.obj.amsT_FatherOfficeAdd = "";
            $scope.obj.amsT_FatherOccupation = "";
            $scope.obj.amsT_FatherDesignation = "";
            $scope.obj.amsT_FatherBankAccNo = "";
            $scope.obj.amsT_FatherBankIFSC_Code = "";
            $scope.obj.amsT_FatherCasteCertiNo = "";
            $scope.obj.IVRMMC_Id3 = "";
            $scope.obj.amsT_FatherMonIncome = "";
            $scope.obj.amsT_FatherAnnIncome = "";
            $scope.obj.amsT_FatherMobleNo = "";
            $scope.obj.amsT_FatheremailId = "";
            $scope.obj.amsT_MotherAliveFlag = "";
            $scope.obj.amsT_MotherName = "";
            $scope.obj.amsT_MotherSurname = "";
            $scope.obj.amsT_MotherAadharNo = "";
            $scope.obj.amsT_MotherEducation = "";
            $scope.obj.amsT_MotherOfficeAdd = "";
            $scope.obj.amsT_MotherOccupation = "";
            $scope.obj.amsT_MotherDesignation = "";
            $scope.obj.amsT_MotherBankAccNo = "";
            $scope.obj.amsT_MotherBankIFSC_Code = "";
            $scope.obj.amsT_MotherCasteCertiNo = "";
            $scope.obj.IVRMMC_Id4 = "";
            $scope.obj.amsT_MotherMonIncome = "";
            $scope.obj.amsT_MotherAnnIncome = "";
            $scope.obj.amsT_MotherMobileNo = "";
            $scope.obj.amsT_MotherEmailId = "";
            $scope.search = "";
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();

            $scope.obj.amsT_MotherSubCaste = "";
            $scope.obj.amsT_MotherCaste = "";
            $scope.obj.amsT_MotherReligion = "";

            $scope.obj.amsT_FatherSubCaste = "";
            $scope.obj.amsT_FatherCaste = "";
            $scope.obj.amsT_FatherReligion = "";


            $scope.mobiles = {};
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.mobiles[0].amcsT_FatherMobleNo = "";

            $scope.emails = {};
            $scope.emails = [{ id: 'email1' }];
            $scope.emails[0].amcsT_FatheremailId = "";

            $scope.mobiles1 = {};
            $scope.mobiles1 = [{ id: 'mobile2' }];
            $scope.mobiles1[0].amcsT_MotherMobleNo = "";

            $scope.emails1 = {};
            $scope.emails1 = [{ id: 'email2' }];
            $scope.emails1[0].amcsT_MotheremailId = "";

        }

        var name = "";

        $scope.clear_fourth_tab = function () {
            $scope.obj.amsteC_Extracurricular = "";
            $scope.search = "";

            for (var i = 0; i < $scope.allActivity.length; i++) {
                $scope.allActivity[i].Selected = false;
            }


            for (var i = 0; i < $scope.allRefrence.length; i++) {
                $scope.allRefrence[i].Selected = false;
            }



            for (var i = 0; i < $scope.allSources.length; i++) {
                $scope.allSources[i].Selected = false;
            }

            for (var i = 0; i < $scope.govtBondList.length; i++) {
                $scope.govtBondList[i].Selected = false;
                $scope.govtBondList[i].amstB_BandNo = "";
            }


            for (var i = 0; i < $scope.prevSchoolDetails.length; i++) {

                $scope.prevSchoolDetails[i].acstpS_PrvSchoolName = "";
                $scope.prevSchoolDetails[i].acstpS_PreSchoolType = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousClass = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousPer = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousGrade = "";
                $scope.prevSchoolDetails[i].acstpS_LeftYear = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousMarks = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousMarksObtained = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousTCNo = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousTCDate = "";
                $scope.prevSchoolDetails[i].acstpS_PreSchoolBoard = "";
                $scope.prevSchoolDetails[i].acstpS_PreSchoolCountry = "";
                $scope.prevSchoolDetails[i].acstpS_PreSchoolState = "";
                $scope.prevSchoolDetails[i].acstpS_Address = "";
                $scope.prevSchoolDetails[i].acstpS_LeftReason = "";
                $scope.prevSchoolDetails[i].acstpS_MediumOfInst = "";

                $scope.prevSchoolDetails[i].acstpS_PreviousBranch = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousExamPassed = "";
                $scope.prevSchoolDetails[i].acstpS_PasssedMonthYear = "";
                $scope.prevSchoolDetails[i].acstpS_LanguagesTaken = "";
                $scope.prevSchoolDetails[i].acstpS_Regno = "";
                $scope.prevSchoolDetails[i].acstpS_PreviousGrade = "";
            }


            for (var i = 0; i < $scope.prevexammarksdetails.length; i++) {
                $scope.prevexammarksdetails[i].acstsuM_SubjectName = "";
                $scope.prevexammarksdetails[i].acstsuM_MaxMarks = "";
                $scope.prevexammarksdetails[i].acstsuM_SubjectMarks = "";
                $scope.prevexammarksdetails[i].acstsuM_Percentage = "";
                $scope.prevexammarksdetails[i].acstsuM_LangFlg = "";
            }






            for (var i = 0; i < $scope.studentGuardianDetails.length; i++) {

                $scope.studentGuardianDetails[i].acstG_GuardianName = "";
                $scope.studentGuardianDetails[i].acstG_GuardianAddress = "";
                $scope.studentGuardianDetails[i].acstG_emailid = "";
                $scope.studentGuardianDetails[i].acstG_GuardianPhoneNo = "";
                $scope.studentGuardianDetails[i].acstG_GuardianPhoto = null;
                $scope.studentGuardianDetails[i].acstG_GuardianSign = null;
                $scope.studentGuardianDetails[i].acstG_Fingerprint = null;



            }

            for (var i = 0; i < $scope.studentSiblingDetails.length; i++) {

                $scope.studentSiblingDetails[i].acstS_SiblingsName = "";
                $scope.studentSiblingDetails[i].amcO_Id = "";
                $scope.studentSiblingDetails[i].acstS_SiblingsRelation = "";

            }

            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();




        }

        $scope.clear_fifth_tab = function () {

            $scope.obj.amsT_HostelReqdFlag = "";
            $scope.obj.amsT_TransportReqdFlag = "";
            $scope.obj.amsT_GymReqdFlag = "";
            $scope.obj.amsT_ECSFlag = "";
            $scope.search = "";

            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });


            for (var i = 0; i < $scope.documentList.length; i++) {


                $scope.documentList[i].document_Path = '';
            }

            $scope.fatherphoto = null;
            $scope.UploadFatherPhoto.fatherSign = null;


            $scope.fatherSign = null;
            $scope.fatherFingerprint = null;
            $scope.motherphoto = null;
            $scope.mothersign = null;
            $scope.motherfingerprint = null;




            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            $scope.submitted4 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };

        $scope.getcaste = function (imcC_Id) {
            if (imcC_Id != "") {
                var data = {
                    "IMCC_Id": imcC_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeStudentAdmission/getCaste", data).then(function (promise) {

                    if (promise.allCaste.length > 0) {
                        $scope.allCaste = promise.allCaste;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.castedisble = true;
                        $scope.obj.IMC_Id = "";
                    }

                })
            }
        }

        $scope.clearAll = function () {
            $state.reload();
        }


        //added by vishnu
        $scope.headinstallmentid = function (totalgrid, index) {

            $scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
            { value: "Day Slab", FTFS_FineType: "Day Slab" }
            ];


            $scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
            { value: "Day Slab", FTFSE_FineType: "Day Slab" }
            ];

            $scope.typeofstudentregular = false;

            headid = totalgrid[index].fmH_Id;
            ftiid = totalgrid[index].ftI_Id;

            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMCC_Id": $scope.FMCC_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntry/getgroupmappedheads", data).
                then(function (promise) {

                    $scope.savedatatransregular = [];
                    $scope.savedatatransecs = [];
                    var r = 0;
                    var e = 0;
                    if (promise.fineslabdetails.length > 0) {
                        $scope.slab1 = true;
                        $scope.slab2 = true;

                        var lenofarray = promise.fineslabdetails.length;

                        for (var i = 0; i < lenofarray; i++) {
                            if (promise.fineslabdetails[i].fmfS_ECSFlag == "R" && promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid) {
                                $scope.savedatatransregular[r] = promise.fineslabdetails[i]
                                r = r + 1;
                            }
                        }

                        var lenofarrayecs = promise.fineslabdetailsecs.length;
                        for (var i = 0; i < lenofarrayecs; i++) {
                            if (promise.fineslabdetailsecs[i].fmfS_ECSFlag == "E" && promise.fineslabdetailsecs[i].fmH_Id == headid && promise.fineslabdetailsecs[i].ftI_Id == ftiid) {
                                $scope.savedatatransecs[e] = promise.fineslabdetailsecs[i]
                                e = e + 1;
                            }
                        }


                        if ($scope.savedatatransregular.length > 0) {
                            $scope.typeofstudentregular = true;
                            $scope.slab1 = true;
                            $scope.slab2 = false;
                            $scope.finslab1 = $scope.savedatatransregular;

                        }
                        if ($scope.savedatatransecs.length > 0) {
                            $scope.typeofstudentecs = true;
                            $scope.slab2 = true;
                            $scope.slab1 = true;
                            $scope.finslab2 = $scope.savedatatransecs;
                        }
                    }
                    else {
                        $scope.slab1 = false;
                        $scope.slab2 = false;
                    }
                })

        }


        $scope.getstudentlistre = function () {
            var data = {
                "ASMAY_Id": $scope.yearid1,
            }
            apiService.create("StudentAdmission/yearwisetcstd/", data).then
                (function (promise) {
                    if (promise != null) {
                        $scope.stdname = promise.studentList1;
                    }
                    else {
                        swal("No Records Found From Selected Year..")
                    }
                });
        }


        $scope.addtocart = function () {

            var data = {
                "ASMAY_Id": $scope.yearid1,
                "AMST_Id": $scope.amsT_Id,
            }
            apiService.create("StudentAdmission/addtocart/", data).then
                (function (promise) {

                    $scope.documentList = promise.documentList;
                    $scope.DOB = false;
                    $scope.mi_id = promise.mI_Id;



                    if (promise.bondList.length > 0 && promise.bondList != null) {
                        $scope.bondList = promise.bondList;
                    }
                    if (promise.prevSchoolDetails.length > 0 && promise.prevSchoolDetails != null) {
                        $scope.prevSchoolDetails = promise.prevSchoolDetails;
                    }
                    if (promise.studentGuardianDetails.length > 0 && promise.studentGuardianDetails != null) {
                        $scope.studentGuardianDetails = promise.studentGuardianDetails;
                    }

                    if (promise.Adm_College_Student_SubjectMarksDTO.length > 0 && promise.Adm_College_Student_SubjectMarksDTO != null) {
                        $scope.prevexammarksdetails = promise.Adm_College_Student_SubjectMarksDTO;
                    }


                    if (promise.studentSiblingDetails.length > 0 && promise.studentSiblingDetails != null) {

                        $scope.studentSiblingDetails = promise.studentSiblingDetails;
                    }

                    $scope.studentReferenceDetails = promise.studentReferenceDetails;
                    $scope.studentSourceDetails = promise.studentSourceDetails;
                    $scope.studentActivityDetails = promise.studentActivityDetails;


                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        $scope.allRefrence[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        name = $scope.allRefrence[i].pamR_Id;
                        for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                            if (name == promise.studentReferenceDetails[j].pamR_Id) {
                                $scope.allRefrence[i].Selected = true;
                            }
                        }
                    }
                    for (var i = 0; i < $scope.allSources.length; i++) {
                        $scope.allSources[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allSources.length; i++) {
                        name = $scope.allSources[i].pamS_Id;
                        for (var j = 0; j < promise.studentSourceDetails.length; j++) {
                            if (name == promise.studentSourceDetails[j].pamS_Id) {
                                $scope.allSources[i].Selected = true;
                            }
                        }
                    }
                    for (var i = 0; i < $scope.allActivity.length; i++) {
                        $scope.allActivity[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allActivity.length; i++) {
                        name = $scope.allActivity[i].amA_Id;
                        for (var j = 0; j < promise.studentActivityDetails.length; j++) {
                            if (name == promise.studentActivityDetails[j].amA_Id) {
                                $scope.allActivity[i].Selected = true;
                            }
                        }
                    }
                    //documnets
                    if (promise.documentList.length > 0) {
                        $scope.document = {};
                        $scope.documentList = promise.documentList;
                        angular.forEach(promise.documentList, function (value, key) {
                            $('#' + value.amsmD_Id).attr('src', value.document_Path);
                        })
                    }


                    $('#blah').attr('src', promise.amsT_Photoname);

                    $scope.Editfatherphoto = promise.ansT_FatherPhoto;
                    $scope.EditfatherSign = promise.amsT_Father_Signature;
                    $scope.EditfatherFingerprint = promise.amsT_Father_FingerPrint;
                    $scope.Editmotherphoto = promise.ansT_MotherPhoto;
                    $scope.Editmothersign = promise.amsT_Mother_Signature;
                    $scope.Editmotherfingerprint = promise.amsT_Mother_FingerPrint;

                    $scope.fatherphoto = promise.studentList[0].ansT_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].amsT_Father_Signature;
                    $scope.fatherFingerprint = promise.studentList[0].amsT_Father_FingerPrint;
                    $scope.motherphoto = promise.studentList[0].ansT_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].amsT_Mother_Signature;
                    $scope.motherfingerprint = promise.studentList[0].amsT_Mother_FingerPrint;
                    $scope.obj.image = promise.studentList[0].amsT_Photoname;

                    $scope.obj.amsT_FirstName = promise.studentList[0].amsT_FirstName;

                    $scope.obj.amsT_MiddleName = promise.studentList[0].amsT_MiddleName;
                    $scope.obj.amsT_LastName = promise.studentList[0].amsT_LastName;
                    $scope.amsT_Date = new Date(promise.studentList[0].amsT_Date);

                    $scope.obj.asmcL_Id = promise.studentList[0].asmcL_Id;
                    if (promise.stud_catg_edit.length > 0) {
                        $scope.obj.stucategory = promise.stud_catg_edit[0].asmcC_Id;
                    }
                    $scope.amsT_Sex = promise.studentList[0].amsT_Sex;
                    $scope.obj.amsT_DOB = new Date(promise.studentList[0].amsT_DOB);
                    $scope.obj.amsT_DOB_Words = promise.studentList[0].amsT_DOB_Words;
                    $scope.obj.pasR_Age = promise.studentList[0].pasR_Age;
                    $scope.obj.amsT_BloodGroup = promise.studentList[0].amsT_BloodGroup;
                    $scope.obj.amsT_MotherTongue = promise.studentList[0].amsT_MotherTongue;
                    $scope.obj.amsT_BirthCertNO = promise.studentList[0].amsT_BirthCertNO;
                    $scope.obj.ivrmmR_Id = promise.studentList[0].ivrmmR_Id;


                    $scope.obj.imcC_Id = promise.studentList[0].imcC_Id;
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        $scope.allCaste[i].Selected = false;
                        $scope.obj.iC_Id = "";
                    }


                    if (promise.allCaste.length > 0) {
                        for (var i = 0; i < promise.allCaste.length; i++) {
                            if (promise.studentList[0].iC_Id == promise.allCaste[i].imC_Id) {
                                $scope.allCaste[i].Selected = true;
                                $scope.obj.iC_Id = promise.studentList[0].iC_Id;
                            }
                        }
                    }
                    else {
                        swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    }

                    $scope.obj.IVRMMC_Id = promise.studentList[0].amsT_Nationality;

                    $scope.obj.IVRMMC_Id5 = promise.studentList[0].amsT_PerCountry;


                    getSelectGetState($scope.obj.IVRMMC_Id5, promise.studentList[0].amsT_PerState);

                    $scope.obj.amsT_PerState = promise.studentList[0].amsT_PerState;

                    $scope.obj.amsT_PerStreet = promise.studentList[0].amsT_PerStreet;
                    $scope.obj.amsT_PerArea = promise.studentList[0].amsT_PerArea;
                    $scope.obj.amsT_PerCity = promise.studentList[0].amsT_PerCity;

                    $scope.obj.amsT_PerPincode = promise.studentList[0].amsT_PerPincode;

                    $scope.obj.amsT_StuBankAccNo = promise.studentList[0].amsT_StuBankAccNo;
                    $scope.obj.amsT_StuBankIFSC_Code = promise.studentList[0].amsT_StuBankIFSC_Code;
                    $scope.obj.amsT_AadharNo = promise.studentList[0].amsT_AadharNo;
                    $scope.obj.amsT_BirthPlace = promise.studentList[0].amsT_BirthPlace;
                    $scope.obj.amsT_StuCasteCertiNo = promise.studentList[0].amsT_StuCasteCertiNo;
                    $scope.obj.amsT_MobileNo = promise.studentList[0].amsT_MobileNo;
                    $scope.obj.amsT_emailId = promise.studentList[0].amsT_emailId;

                    $scope.obj.amsT_PerStreet = promise.studentList[0].amsT_PerStreet;
                    $scope.obj.amsT_ConPincode = promise.studentList[0].amsT_ConPincode;
                    $scope.obj.amsT_ConArea = promise.studentList[0].amsT_ConArea;
                    $scope.obj.amsT_ConStreet = promise.studentList[0].amsT_ConStreet;
                    $scope.obj.amsT_ConCity = promise.studentList[0].amsT_ConCity;
                    $scope.obj.amsT_ConCountry = promise.studentList[0].amsT_ConCountry;

                    getSelectGetState2($scope.obj.amsT_ConCountry, promise.studentList[0].amsT_ConState);

                    $scope.obj.amsT_ConState = promise.studentList[0].amsT_ConState;




                    if (promise.studentList[0].amsT_FatherAliveFlag === "true") {
                        $scope.obj.amsT_FatherAliveFlag = true;
                    }
                    else {
                        $scope.obj.amsT_FatherAliveFlag = false;
                    }

                    $scope.obj.amsT_FatherName = promise.studentList[0].amsT_FatherName;
                    $scope.obj.amsT_FatherSurname = promise.studentList[0].amsT_FatherSurname;
                    $scope.obj.amsT_FatherAadharNo = promise.studentList[0].amsT_FatherAadharNo;
                    $scope.obj.amsT_FatherEducation = promise.studentList[0].amsT_FatherEducation;
                    $scope.obj.amsT_FatherOfficeAdd = promise.studentList[0].amsT_FatherOfficeAdd;
                    $scope.obj.amsT_FatherOccupation = promise.studentList[0].amsT_FatherOccupation;
                    $scope.obj.amsT_FatherDesignation = promise.studentList[0].amsT_FatherDesignation;
                    $scope.obj.amsT_FatherBankAccNo = promise.studentList[0].amsT_FatherBankAccNo;
                    $scope.obj.amsT_FatherBankIFSC_Code = promise.studentList[0].amsT_FatherBankIFSC_Code;
                    $scope.obj.amsT_FatherCasteCertiNo = promise.studentList[0].amsT_FatherCasteCertiNo;
                    $scope.obj.IVRMMC_Id3 = promise.studentList[0].amsT_FatherNationality;
                    $scope.obj.amsT_FatherMonIncome = promise.studentList[0].amsT_FatherMonIncome;
                    $scope.obj.amsT_FatherAnnIncome = promise.studentList[0].amsT_FatherAnnIncome;
                    $scope.obj.amsT_FatherMobleNo = promise.studentList[0].amsT_FatherMobleNo;
                    $scope.obj.amsT_FatheremailId = promise.studentList[0].amsT_FatheremailId;



                    if (promise.studentList[0].amsT_MotherAliveFlag === "true") {
                        $scope.obj.amsT_MotherAliveFlag = true;
                    }
                    else {
                        $scope.obj.amsT_MotherAliveFlag = false;
                    }

                    $scope.obj.amsT_MotherName = promise.studentList[0].amsT_MotherName;
                    $scope.obj.amsT_MotherSurname = promise.studentList[0].amsT_MotherSurname;
                    $scope.obj.amsT_MotherAadharNo = promise.studentList[0].amsT_MotherAadharNo;
                    $scope.obj.amsT_MotherEducation = promise.studentList[0].amsT_MotherEducation;
                    $scope.obj.amsT_MotherOfficeAdd = promise.studentList[0].amsT_MotherOfficeAdd;
                    $scope.obj.amsT_MotherOccupation = promise.studentList[0].amsT_MotherOccupation;
                    $scope.obj.amsT_MotherDesignation = promise.studentList[0].amsT_MotherDesignation;
                    $scope.obj.amsT_MotherBankAccNo = promise.studentList[0].amsT_MotherBankAccNo;
                    $scope.obj.amsT_MotherBankIFSC_Code = promise.studentList[0].amsT_MotherBankIFSC_Code;
                    $scope.obj.amsT_MotherCasteCertiNo = promise.studentList[0].amsT_MotherCasteCertiNo;
                    $scope.obj.IVRMMC_Id4 = promise.studentList[0].amsT_MotherNationality;
                    $scope.obj.amsT_MotherMonIncome = promise.studentList[0].amsT_MotherMonIncome;
                    $scope.obj.amsT_MotherAnnIncome = promise.studentList[0].amsT_MotherAnnIncome;
                    $scope.obj.amsT_MotherMobileNo = promise.studentList[0].amsT_MotherMobileNo;
                    $scope.obj.amsT_MotherEmailId = promise.studentList[0].amsT_MotherEmailId;

                    if (promise.studentAchivementDetails.length > 0) {
                        $scope.obj.amsteC_Extracurricular = promise.studentAchivementDetails[0].amsteC_Extracurricular;
                    }

                    $scope.obj.amsT_BPLCardFlag = promise.studentList[0].amsT_BPLCardFlag;
                    if (promise.studentList[0].amsT_BPLCardFlag == 1) {

                        $scope.obj.amsT_BPLCardFlag = true;
                    }
                    else {
                        $scope.obj.amsT_BPLCardFlag = false;
                    }

                    $scope.obj.amsT_BPLCardNo = promise.studentList[0].amsT_BPLCardNo;
                    $scope.obj.amsT_HostelReqdFlag = promise.studentList[0].amsT_HostelReqdFlag;
                    if ($scope.obj.amsT_HostelReqdFlag === 1) {
                        $scope.obj.amsT_HostelReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_HostelReqdFlag = false;
                    }
                    $scope.obj.amsT_TransportReqdFlag = promise.studentList[0].amsT_TransportReqdFlag;
                    if ($scope.obj.amsT_TransportReqdFlag === 1) {
                        $scope.obj.amsT_TransportReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_TransportReqdFlag = false;
                    }
                    $scope.obj.amsT_GymReqdFlag = promise.studentList[0].amsT_GymReqdFlag;
                    if ($scope.obj.amsT_GymReqdFlag === 1) {
                        $scope.obj.amsT_GymReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_GymReqdFlag = false;
                    }
                    $scope.obj.amsT_ECSFlag = promise.studentList[0].amsT_ECSFlag;
                    if ($scope.obj.amsT_ECSFlag === 1) {
                        $scope.obj.amsT_ECSFlag = true;
                    }
                    else {
                        $scope.obj.amsT_ECSFlag = false;
                    }
                    $('#myModalreadmit').modal('hide');
                    $scope.readmit = false;
                    $scope.readmitreload();
                    $scope.amsT_Id = "";
                });
        }

        $scope.removeall = function () {
            $('#myModalreadmit').modal('hide');
            $scope.readmit = false;
            $scope.readmitreload();
            $scope.amsT_Id = "";
        }
        $scope.readmitreload = function () {
            apiService.getDATA("StudentAdmission/Getdetails").then(
                function (promise) {
                    $scope.yearre = promise.academicyearforreadmit;

                });
        }

        $scope.searchByColumn = function (search, searchColumn, searchyear) {

            if (search != "" && search != null && search != undefined) {
                if (searchyear == null || searchyear == "") {
                    searchyear = 0;
                }
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                    "ASMAY_Id": searchyear
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeStudentAdmission/SearchByColumn", data)

                    .then(function (promise) {

                        if (promise.count == 0) {
                            swal("No Records Found");
                            $state.reload();
                        }
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            if (promise.studentList != null && promise.studentList != "") {
                                for (var i = 0; i < promise.studentList.length; i++) {

                                    if (promise.studentList[i].amcsT_MiddleName == null || promise.studentList[i].amcsT_MiddleName == '' || promise.studentList[i].amcsT_MiddleName == "") {
                                        promise.studentList[i].amcsT_MiddleName = "Not Available";
                                    }

                                    if (promise.studentList[i].amcsT_LastName == null || promise.studentList[i].amcsT_LastName == '' || promise.studentList[i].amcsT_LastName == "") {
                                        promise.studentList[i].amcsT_LastName = "Not Available";
                                    }

                                    if (promise.studentList[i].amcsT_SOL == "S") {
                                        promise.studentList[i].amcsT_SOL = "Active";
                                    }
                                    else if (promise.studentList[i].amcsT_SOL == "D") {
                                        promise.studentList[i].amcsT_SOL = "Deactive";
                                    }
                                    else if (promise.studentList[i].amcsT_SOL == "L") {
                                        promise.studentList[i].amcsT_SOL = "Left";
                                    }

                                    //if (promise.studentList[i].amsT_SOL == "S") {
                                    //    promise.studentList[i].amsT_SOL = "Active";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "D") {
                                    //    promise.studentList[i].amsT_SOL = "Deactive";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "L") {
                                    //    promise.studentList[i].amsT_SOL = "Left";
                                    //}
                                }
                            }
                            $scope.StudentList = promise.studentList;
                            $scope.presentCountgrid = $scope.StudentList.length;

                        }
                        else {
                            $scope.search = "";
                            if (promise.studentList != null && promise.studentList != "") {
                                for (var i = 0; i < promise.studentList.length; i++) {
                                    if (promise.studentList[i].amcsT_MiddleName == null || promise.studentList[i].amcsT_MiddleName == '' || promise.studentList[i].amcsT_MiddleName == "") {
                                        promise.studentList[i].amcsT_MiddleName = "";
                                    }

                                    if (promise.studentList[i].amcsT_LastName == null || promise.studentList[i].amcsT_LastName == '' || promise.studentList[i].amcsT_LastName == "") {
                                        promise.studentList[i].amcsT_LastName = "";
                                    }

                                    if (promise.studentList[i].amcsT_SOL == "S") {
                                        promise.studentList[i].amcsT_SOL = "Active";
                                    }
                                    else if (promise.studentList[i].amcsT_SOL == "D") {
                                        promise.studentList[i].amcsT_SOL = "Deactive";
                                    }
                                    else if (promise.studentList[i].amcsT_SOL == "L") {
                                        promise.studentList[i].amcsT_SOL = "Left";
                                    }

                                    //if (promise.studentList[i].amsT_SOL == "S") {
                                    //    promise.studentList[i].amsT_SOL = "Active";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "D") {
                                    //    promise.studentList[i].amsT_SOL = "Deactive";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "L") {
                                    //    promise.studentList[i].amsT_SOL = "Left";
                                    //}
                                }
                            }
                            $scope.StudentList = promise.studentList;
                            $scope.presentCountgrid = $scope.StudentList.length;
                        }

                    })
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }

        $scope.getcolumnId = function (ColumnId) {
            if (ColumnId == "14") {
                swal("Enter data in for ex: 2015-2016 format");
            }
        }
        $scope.guardianNameEntered = function (guardianDet, index) {

            if ($scope.EditId > 0) {
                if ($scope.grddetcount == 0) {
                    if ($scope.studentGuardianDetails[index].acstG_GuardianName == "" || $scope.studentGuardianDetails[index].acstG_GuardianName == null || $scope.studentGuardianDetails[index].acstG_GuardianName == undefined) {
                        $scope.studentGuardianDetails[index].acstG_GuardianAddress = "";
                        $scope.studentGuardianDetails[index].acstG_emailid = "";
                        $scope.studentGuardianDetails[index].acstG_GuardianPhoneNo = "";
                    }
                }
                else {
                    if ($scope.studentGuardianDetails[index].acstG_GuardianName == "" || $scope.studentGuardianDetails[index].acstG_GuardianName == null || $scope.studentGuardianDetails[index].acstG_GuardianName == undefined) {
                        $scope.errorMessage = 'Guardian Name Required';
                    }
                    else {
                        $scope.errorMessage = '';
                    }
                }
            }
            else {
                if ($scope.studentGuardianDetails[index].acstG_GuardianName == "" || $scope.studentGuardianDetails[index].acstG_GuardianName == null || $scope.studentGuardianDetails[index].acstG_GuardianName == undefined) {
                    $scope.studentGuardianDetails[index].acstG_GuardianAddress = "";
                    $scope.studentGuardianDetails[index].acstG_emailid = "";
                    $scope.studentGuardianDetails[index].acstG_GuardianPhoneNo = "";
                }
            }
        }
        $scope.siblingNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.sibcount == 0) {
                    if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                        $scope.studentSiblingDetails[index].amcO_Id = "";
                        $scope.studentSiblingDetails[index].acstS_SiblingsRelation = "";
                    }
                }
                else {
                    if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                        $scope.errorMessage1 = 'Sibling Name Required';
                    }
                    else {
                        $scope.errorMessage1 = '';
                    }
                }
            }
            else {
                if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                    $scope.studentSiblingDetails[index].amcO_Id = "";
                    $scope.studentSiblingDetails[index].acstS_SiblingsRelation = "";

                }
            }
        }
        $scope.prevSchoolNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.prevschlcount == 0) {
                    if ($scope.prevSchoolDetails[index].acstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == undefined) {
                        $scope.prevSchoolDetails[index].acstpS_PreSchoolType = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousClass = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousPer = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousGrade = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousTCNo = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousTCDate = "";
                        $scope.prevSchoolDetails[index].acstpS_LeftYear = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousMarks = "";
                        $scope.prevSchoolDetails[index].acstpS_PreviousMarksObtained = "";
                        $scope.prevSchoolDetails[index].acstpS_PreSchoolBoard = "";
                        $scope.prevSchoolDetails[index].acstpS_PreSchoolCountry = "";
                        $scope.prevSchoolDetails[index].acstpS_PreSchoolState = "";
                        $scope.prevSchoolDetails[index].acstpS_Address = "";
                        $scope.prevSchoolDetails[index].acstpS_LeftReason = "";
                        $scope.prevSchoolDetails[index].acstpS_MediumOfInst = "";
                        $scope.prevSchoolDetails[i].acstpS_PreviousBranch = "";
                        $scope.prevSchoolDetails[i].acstpS_PreviousExamPassed = "";
                        $scope.prevSchoolDetails[i].acstpS_PasssedMonthYear = "";
                        $scope.prevSchoolDetails[i].acstpS_LanguagesTaken = "";
                        $scope.prevSchoolDetails[i].acstpS_Regno = "";
                        $scope.prevSchoolDetails[i].acstpS_PreviousGrade = "";
                    }
                }
                else {
                    if ($scope.prevSchoolDetails[index].acstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == undefined) {
                        $scope.errorMessage2 = 'School Name Required';
                    }
                    else {
                        $scope.errorMessage2 = '';
                    }
                }
            }
            else {
                if ($scope.prevSchoolDetails[index].acstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].acstpS_PrvSchoolName == undefined) {
                    $scope.prevSchoolDetails[index].acstpS_PreSchoolType = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousClass = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousPer = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousGrade = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousTCNo = "";
                    $scope.prevSchoolDetails[index].acstpS_LeftYear = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousMarks = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousMarksObtained = "";
                    $scope.prevSchoolDetails[index].acstpS_PreviousTCDate = "";

                    $scope.prevSchoolDetails[index].acstpS_PreSchoolBoard = "";
                    $scope.prevSchoolDetails[index].acstpS_PreSchoolCountry = "";
                    $scope.prevSchoolDetails[index].acstpS_PreSchoolState = "";
                    $scope.prevSchoolDetails[index].acstpS_Address = "";
                    $scope.prevSchoolDetails[index].acstpS_LeftReason = "";
                    $scope.prevSchoolDetails[index].acstpS_MediumOfInst = "";

                }
            }
        }

        $scope.prevSchoolsubjectEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.prevexammarksdetailscount == 0) {

                    if ($scope.prevexammarksdetails[index].acstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].acstsuM_SubjectName == null || $scope.prevexammarksdetails[index].acstsuM_SubjectName == undefined) {

                        $scope.prevexammarksdetails[index].acstsuM_MaxMarks = "";
                        $scope.prevexammarksdetails[index].acstsuM_SubjectMarks = "";
                        $scope.prevexammarksdetails[index].acstsuM_Percentage = "";
                        $scope.prevexammarksdetails[index].acstsuM_LangFlg = "";
                    }
                }
                else {
                    if ($scope.prevexammarksdetails[index].acstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].acstsuM_SubjectName == null || $scope.prevexammarksdetails[index].acstsuM_SubjectName == undefined) {
                        $scope.errorMessage21 = 'Subject Name Required';
                    }
                    else {
                        $scope.errorMessage21 = '';
                    }
                }
            }
            else {
                if ($scope.prevexammarksdetails[index].acstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].acstsuM_SubjectName == null || $scope.prevexammarksdetails[index].acstsuM_SubjectName == undefined) {
                    $scope.prevexammarksdetails[index].acstsuM_MaxMarks = "";
                    $scope.prevexammarksdetails[index].acstsuM_SubjectMarks = "";
                    $scope.prevexammarksdetails[index].acstsuM_Percentage = "";
                    $scope.prevexammarksdetails[index].acstsuM_LangFlg = "";
                }
            }
        };



        $scope.onselectprevCountry = function (acstpS_PreSchoolCountry) {
            var countryname = {
                "countryName": acstpS_PreSchoolCountry
            }

            apiService.create("CollegeStudentAdmission/StateByCountryName/", countryname).then(function (promise) {

                $scope.prevState = promise.prevStateList;

            })
        }

        //get month names
        //month names
        $scope.getmontnames = function (monthid) {

            monthname = "";
            switch (monthid) {

                case 0:
                    monthname = "JANUARY"
                    break;
                case 1:
                    monthname = "FEBRUARY"
                    break;
                case 2:
                    monthname = "MARCH"
                    break;
                case 3:
                    monthname = "APRIL"
                    break;
                case 4:
                    monthname = "MAY"
                    break;
                case 5:
                    monthname = "JUNE"
                    break;
                case 6:
                    monthname = "JULY"
                    break;
                case 7:
                    monthname = "AUGUST"
                    break;
                case 8:
                    monthname = "SEPTEMBER"
                    break;
                case 9:
                    monthname = "OCTOBER"
                    break;
                case 10:
                    monthname = "NOVEMBER"
                    break;
                case 11:
                    monthname = "DECEMBER"
                    break;
                default:
                    monthname = ""
                    break;
            }
            return monthname;
        }

        //get datename

        $scope.getdatenames = function (dateid) {

            datename = "";
            switch (dateid) {

                case 1:
                    datename = "FIRST"
                    break;
                case 2:
                    datename = "SECOND"
                    break;
                case 3:
                    datename = "THIRD"
                    break;
                case 4:
                    datename = "FOURTH"
                    break;
                case 5:
                    datename = "FIFTH"
                    break;
                case 6:
                    datename = "SIXTH"
                    break;
                case 7:
                    datename = "SEVENTH"
                    break;
                case 8:
                    datename = "EIGHTH"
                    break;
                case 9:
                    datename = "NINTH"
                    break;
                case 10:
                    datename = "TENTH"
                    break;
                case 11:
                    datename = "ELEVENTH"
                    break;
                case 12:
                    datename = "TWELFTH"
                    break;
                case 13:
                    datename = "THIRTEENTH"
                    break;
                case 14:
                    datename = "FOURTEENTH"
                    break;
                case 15:
                    datename = "FIFTEENTH"
                    break;
                case 16:
                    datename = "SIXTEENTH"
                    break;
                case 17:
                    datename = "SEVENTEENTH"
                    break;
                case 18:
                    datename = "EIGHTEENTH"
                    break;
                case 19:
                    datename = "NINTEENTH"
                    break;
                case 20:
                    datename = "TWENTY"
                    break;
                case 21:
                    datename = "TWENTY FIRST"
                    break;
                case 22:
                    datename = "TWENTY SECOND"
                    break;
                case 23:
                    datename = "TWENTY THIRD"
                    break;
                case 24:
                    datename = "TWENTY FOURTH"
                    break;
                case 25:
                    datename = "TWENTY FIFTH"
                    break;
                case 26:
                    datename = "TWENTY SIXTH"
                    break;
                case 27:
                    datename = "TWENTY SEVENTH"
                    break;
                case 28:
                    datename = "TWENTY EIGHTH"
                    break;
                case 29:
                    datename = "TWENTY NINTH"
                    break;
                case 30:
                    datename = "THIRTY"
                    break;
                case 31:
                    datename = "THIRTY FIRST"
                    break;

                default:
                    datename = ""
                    break;
            }
            return datename;

        }


        //getting year 1
        $scope.getyearname = function (yearid) {

            var yearid = yearid.toString();

            var yearsplit = yearid.substring(0, 2)
            var dob = parseInt(yearsplit);

            var yearsplit1 = yearid.substring(2, 4)
            var dob1 = parseInt(yearsplit1);

            yearname = "";
            var datad = yearid;
            var yearname1 = "";





            if (dob >= 20) {
                yearname = "TWO THOUSAND";
            }
            else {
                yearname = "NINTEEN";
            }
            switch (dob1) {

                case 1:
                    yearname1 = "ONE"
                    break;
                case 2:
                    yearname1 = "TWO"
                    break;
                case 3:
                    yearname1 = "THREE"
                    break;
                case 4:
                    yearname1 = "FOUR"
                    break;
                case 5:
                    yearname1 = "FIVE"
                    break;
                case 6:
                    yearname1 = "SIX"
                    break;
                case 7:
                    yearname1 = "SEVEN"
                    break;
                case 8:
                    yearname1 = "EIGHT"
                    break;
                case 9:
                    yearname1 = "NINE"
                    break;
                case 10:
                    yearname1 = "TEN"
                    break;
                case 11:
                    yearname1 = "ELEVEN"
                    break;
                case 12:
                    yearname1 = "TWELVE"
                    break;
                case 13:
                    yearname1 = "THIRTEEN"
                    break;
                case 14:
                    yearname1 = "FOURTEEN"
                    break;
                case 15:
                    yearname1 = "FIFTEEN"
                    break;
                case 16:
                    yearname1 = "SIXTEEN"
                    break;
                case 17:
                    yearname1 = "SEVENTEEN"
                    break;
                case 18:
                    yearname1 = "EIGHTEEN"
                    break;
                case 19:
                    yearname1 = "NINTEEN"
                    break;
                case 20:
                    yearname1 = "TWENTY"
                    break;
                case 21:
                    yearname1 = "TWENTY ONE"
                    break;
                case 22:
                    yearname1 = "TWENTY TWO"
                    break;
                case 23:
                    yearname1 = "TWENTY THREE"
                    break;
                case 24:
                    yearname1 = "TWENTY FOUR"
                    break;
                case 25:
                    yearname1 = "TWENTY FIVE"
                    break;
                case 26:
                    yearname1 = "TWENTY SIX"
                    break;
                case 27:
                    yearname1 = "TWENTY SEVEN"
                    break;
                case 28:
                    yearname1 = "TWENTY EIGHT"
                    break;
                case 29:
                    yearname1 = "TWENTY NINE"
                    break;
                case 30:
                    yearname1 = "THIRTY"
                    break;
                case 31:
                    yearname1 = "THIRTY ONE"
                    break;
                case 32:
                    yearname1 = "THIRTY TWO"
                    break;
                case 33:
                    yearname1 = "THIRTY THREE"
                    break;
                case 34:
                    yearname1 = "THIRTY FOUR"
                    break;
                case 35:
                    yearname1 = "THIRTY FIVE"
                    break;
                case 36:
                    yearname1 = "THIRTY SIX"
                    break;
                case 37:
                    yearname1 = "THIRTY SEVEN"
                    break;
                case 38:
                    yearname1 = "THIRTY EIGHT"
                    break;
                case 39:
                    yearname1 = "THIRTY NINE"
                    break;
                case 40:
                    yearname1 = "FOURTY"
                    break;
                case 41:
                    yearname1 = "FOURTY ONE"
                    break;
                case 42:
                    yearname1 = "FOURTY TWO"
                    break;
                case 43:
                    yearname1 = "FOURTY THREE"
                    break;
                case 44:
                    yearname1 = "FOURTY FOUR"
                    break;
                case 45:
                    yearname1 = "FOURTY FIVE"
                    break;
                case 46:
                    yearname1 = "FOURTY SIX"
                    break;
                case 47:
                    yearname1 = "FOURTY SEVEN"
                    break;
                case 48:
                    yearname1 = "FOURTY EIGHT"
                    break;
                case 49:
                    yearname1 = "FOURTY NINE"
                    break;
                case 50:
                    yearname1 = "FIFTY"
                    break;
                case 51:
                    yearname1 = "FIFTY ONE"
                    break;
                case 52:
                    yearname1 = "FIFTY TWO"
                    break;
                case 53:
                    yearname1 = "FIFTY THREE"
                    break;
                case 54:
                    yearname1 = "FIFTY FOUR"
                    break;
                case 55:
                    yearname1 = "FIFTY FIVE"
                    break;
                case 56:
                    yearname1 = "FIFTY SIX"
                    break;
                case 57:
                    yearname1 = "FIFTY SEVEN"
                    break;
                case 58:
                    yearname1 = "FIFTY EIGHT"
                    break;
                case 59:
                    yearname1 = "FIFTY NINE"
                    break;
                case 60:
                    yearname1 = "SIXTY"
                    break;

                case 61:
                    yearname1 = "SIXTY ONE"
                    break;
                case 62:
                    yearname1 = "SIXTY TWO"
                    break;
                case 63:
                    yearname1 = "SIXTY THREE"
                    break;
                case 64:
                    yearname1 = "SIXTY FOUR"
                    break;
                case 65:
                    yearname1 = "SIXTY FIVE"
                    break;
                case 66:
                    yearname1 = "SIXTY SIX"
                    break;
                case 67:
                    yearname1 = "SIXTY SEVEN"
                    break;
                case 68:
                    yearname1 = "SIXTY EIGHT"
                    break;
                case 69:
                    yearname1 = "SIXTY NINE"
                    break;
                case 70:
                    yearname1 = "SEVENTY"
                    break;

                case 71:
                    yearname1 = "SEVENTY ONE"
                    break;
                case 72:
                    yearname1 = "SEVENTY TWO"
                    break;
                case 73:
                    yearname1 = "SEVENTY THREE"
                    break;
                case 74:
                    yearname1 = "SEVENTY FOUR"
                    break;
                case 75:
                    yearname1 = "SEVENTY FIVE"
                    break;
                case 76:
                    yearname1 = "SEVENTY SIX"
                    break;
                case 77:
                    yearname1 = "SEVENTY SEVEN"
                    break;
                case 78:
                    yearname1 = "SEVENTY EIGHT"
                    break;
                case 79:
                    yearname1 = "SEVENTY NINE"
                    break;
                case 80:
                    yearname1 = "EIGHTY"
                    break;

                case 81:
                    yearname1 = "EIGHTY ONE"
                    break;
                case 82:
                    yearname1 = "EIGHTY TWO"
                    break;
                case 83:
                    yearname1 = "EIGHTY THREE"
                    break;
                case 84:
                    yearname1 = "EIGHTY FOUR"
                    break;
                case 85:
                    yearname1 = "EIGHTY FIVE"
                    break;
                case 86:
                    yearname1 = "EIGHTY SIX"
                    break;
                case 87:
                    yearname1 = "EIGHTY SEVEN"
                    break;
                case 88:
                    yearname1 = "EIGHTY EIGHT"
                    break;
                case 89:
                    yearname1 = "EIGHTY NINE"
                    break;
                case 90:
                    yearname1 = "NINTY"
                    break;

                case 91:
                    yearname1 = "NINTY ONE"
                    break;
                case 92:
                    yearname1 = "NINTY TWO"
                    break;
                case 93:
                    yearname1 = "NINTY THREE"
                    break;
                case 94:
                    yearname1 = "NINTY FOUR"
                    break;
                case 95:
                    yearname1 = "NINTY FIVE"
                    break;
                case 96:
                    yearname1 = "NINTY SIX"
                    break;
                case 97:
                    yearname1 = "NINTY SEVEN"
                    break;
                case 98:
                    yearname1 = "NINTY EIGHT"
                    break;
                case 99:
                    yearname1 = "NINTY NINE"
                    break;
                case 0:
                    yearname1 = "ZERO"
                    break;

                default:
                    yearname1 = ""
                    break;
            }
            yearname = yearname + ' ' + yearname1;
            return yearname;
        }

        //adding multiple mobile number father

        $scope.mobiles = [{ id: 'mobile1' }];
        $scope.selmobs = [{ id: 'selmobile1' }];

        $scope.addNewMobile = function () {
            var newItemNo = $scope.mobiles.length + 1;
            if (newItemNo <= 5) {
                $scope.mobiles.push({ 'id': 'mobile' + newItemNo });
            }
            var newItemNo1 = $scope.selmobs.length + 1;
            if (newItemNo1 <= 5) {
                $scope.selmobs.push({ 'id': 'selmobile' + newItemNo1 });
            }

        };

        //removing mobile number father
        $scope.delm = [];
        $scope.removeNewMobile = function (index, curval1) {


            var newItemNo = $scope.mobiles.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.mobiles.splice(index, 1);


            }
        };

        //adding email id  father

        $scope.emails = [{ id: 'email1' }];
        $scope.addNewEmail = function () {
            var newItemNo = $scope.emails.length + 1;
            if (newItemNo <= 5) {
                $scope.emails.push({ 'id': 'email1' + newItemNo });
            }
        };

        $scope.removeNewEmail = function (index) {


            var newItemNo = $scope.emails.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.emails.splice(index, 1);

            }
        };

        $scope.showAddEmail = function (email) {
            return email.id === $scope.emails[$scope.emails.length - 1].id;
        };

        //adding mobile number mother
        $scope.mobiles1 = [{ id: 'mobile2' }];
        $scope.selmobs1 = [{ id: 'selmobile2' }];

        $scope.addNewMobile1 = function () {
            var newItemNo2 = $scope.mobiles1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.mobiles1.push({ 'id': 'mobile2' + newItemNo2 });
            }
            var newItemNo3 = $scope.selmobs1.length + 1;
            if (newItemNo3 <= 5) {
                $scope.selmobs1.push({ 'id': 'selmobile2' + newItemNo3 });
            }
        };

        //removing mobile number mother
        $scope.delm1 = [];
        $scope.removeNewMobile1 = function (index, curval11) {

            var newItemNo2 = $scope.mobiles1.length - 1;
            if (newItemNo2 !== 0) {
                $scope.delm1 = $scope.mobiles1.splice(index, 1);


            }

        };


        //adding email id mother

        $scope.emails1 = [{ id: 'email2' }];
        $scope.addNewEmail1 = function () {
            var newItemNo2 = $scope.emails1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.emails1.push({ 'id': 'email' + newItemNo2 });
            }
        };

        $scope.removeNewEmail1 = function (index) {


            var newItemNo = $scope.emails1.length - 1;
            if (newItemNo !== 0) {
                $scope.delm1 = $scope.emails1.splice(index, 1);

            }
        };

        $scope.showAddEmail1 = function (email) {
            return email.id === $scope.emails1[$scope.emails1.length - 1].id;
        };

        //adding mobile number student
        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.selmobsstd = [{ id: 'selmobilestd1' }];

        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd1' + newItemNostd });
            }

            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }



        };


        $scope.addtopdays = function (user, index) {

            for (var k = 0; k < $scope.mobilesstd.length; k++) {
                var roll = parseInt(user.amcsT_MobileNo);
                var arryind = $scope.mobilesstd.indexOf($scope.mobilesstd[k]);
                console.log(arryind);
                if (arryind != index) {
                    if ($scope.mobilesstd[k].amcsT_MobileNo == roll) {
                        swal("Already Exist");
                        $scope.mobilesstd[index].amcsT_MobileNo = "";


                        break;
                    }

                }


            }
        };

        //removing mobile number student
        $scope.delmsrd = [];
        $scope.removeNewMobile1std = function (index, curval1std) {

            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);

            }

        };


        //adding email id student

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd1' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index) {


            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);


            }

        };

        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };


        //checking reg no duplicate
        $scope.checkregnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "AMCST_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "AMCST_RegistrationNo": $scope.obj.AMCST_RegistrationNo
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStudentAdmission/checkDuplicate", data).then(function (promise) {


                //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                if (promise.duplicateRegNo > 0) {
                    $scope.obj.AMCST_RegistrationNo = "";
                    swal("Student Reg.No. Already Exists");
                    return;
                }
            });
        }

        $scope.checkadmnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "AMCST_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "AMCST_AdmNo": $scope.obj.AMCST_AdmNo
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeStudentAdmission/checkDuplicate", data).then(function (promise) {


                //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.                
                if (promise.duplicateAdmNo > 0) {
                    $scope.obj.AMCST_AdmNo = "";
                    swal("Student Admission No. Already Exists");
                    return;
                }
            });
        }

        $scope.removeall1 = function () {
            $('#myModalecs').modal('hide');
            $scope.submittedecs = false;
            $scope.myFormecs.$setPristine();
            $scope.myFormecs.$setUntouched();
        }

        $scope.objee = [];
        $scope.objj = [];
        $scope.ecsdetailscheck = function (objj) {
            if (objj == false) {
                swal({
                    title: "Do You Want To Delete The ECS Details Of This Student",
                    text: "Do you want to continue !!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                    cancelButtonText: "Cancel!!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $('#myModalecs').modal('hide');
                        $scope.ecsdetailslist = [];
                        $scope.submittedecs = false;
                        $scope.myFormecs.$setPristine();
                        $scope.myFormecs.$setUntouched();

                    }
                    else {
                        $('#myModalecs').modal('show');
                        $scope.obj.amsT_ECSFlag = true;
                    }
                });

            } else {

            }

        }
        $scope.interactedecs = function (field) {
            return $scope.submittedecs || field.$dirty;
        };
        $scope.submittedecs = false;
        $scope.ecsdetailslist = [];

        $scope.addtocartecs = function (objee) {
            if ($scope.myFormecs.$valid) {
                if (objee.asecS_Id == undefined) {
                    objee.asecS_Id = 0;
                }
                $scope.ecsdetailslist = objee;
                $('#myModalecs').modal('hide');
            }
            else {
                $scope.submittedecs = true;
            }
        }



        //----Saving Tab Wise---------//

        //First Tab
        $scope.savefirsttab = function () {

            if ($scope.myForm1.$valid) {
                var AMCST_BPLCardFlag = false;
                var AMCST_ECSFlag = false;
                var id = 0;
                if ($scope.obj.AMCST_BPLCardFlag == true) {
                    AMCST_BPLCardFlag = true;
                } else {
                    AMCST_BPLCardFlag = false;
                }

                if ($scope.obj.AMCST_ECSFlag === true) {
                    AMCST_ECSFlag = true;
                }
                else {
                    AMCST_ECSFlag = false;
                }
                if ($scope.obj.amcsT_MobileNo == undefined) {
                    $scope.obj.amcsT_MobileNo = 0;
                }

                if ($scope.obj.AMCST_FirstName == null) {
                    $scope.obj.AMCST_FirstName = '';
                } else {
                    $scope.obj.AMCST_FirstName = $scope.obj.AMCST_FirstName;
                }

                if ($scope.obj.AMCST_MiddleName == null) {
                    $scope.obj.AMCST_MiddleName = '';
                } else {
                    $scope.obj.AMCST_MiddleName = $scope.obj.AMCST_MiddleName;
                }

                if ($scope.obj.AMCST_LastName == null) {
                    $scope.obj.AMCST_LastName = '';
                } else {
                    $scope.obj.AMCST_LastName = $scope.obj.AMCST_LastName;
                }

                if ($scope.obj.AMCST_RegistrationNo == null) {
                    $scope.obj.AMCST_RegistrationNo = '';
                } else {
                    $scope.obj.AMCST_RegistrationNo = $scope.obj.AMCST_RegistrationNo;
                }

                if ($scope.obj.AMCST_District == null) {
                    $scope.obj.AMCST_District = '';
                } else {
                    $scope.obj.AMCST_District = $scope.obj.AMCST_District;
                }
                if ($scope.obj.AMCST_Village == null) {
                    $scope.obj.AMCST_Village = '';
                } else {
                    $scope.obj.AMCST_Village = $scope.obj.AMCST_Village;
                }
                if ($scope.obj.AMCST_Taluk == null) {
                    $scope.obj.AMCST_Taluk = '';
                } else {
                    $scope.obj.AMCST_Taluk = $scope.obj.AMCST_Taluk;
                }
                var AMCST_Divyangjan = false;
                if ($scope.obj.AMCST_Divyangjan == true) {
                    AMCST_Divyangjan = true;
                } else {
                    AMCST_Divyangjan = false;
                }




                var AMCST_Date = new Date($scope.obj.AMCST_Date).toDateString();
                var AMCST_DOB = new Date($scope.obj.AMCST_DOB).toDateString();
                if ($scope.EditId == undefined) {
                    id = 0;
                } else {
                    id = $scope.EditId;
                }

                var data = {
                    "AMCST_Id": id,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "AMCST_FirstName": $scope.obj.AMCST_FirstName,
                    "AMCST_MiddleName": $scope.obj.AMCST_MiddleName,
                    "AMCST_LastName": $scope.obj.AMCST_LastName,
                    "AMCST_RegistrationNo": $scope.obj.AMCST_RegistrationNo,
                    "AMCST_AdmNo": $scope.obj.AMCST_AdmNo,
                    "AMCO_Id": $scope.obj.AMCO_Id,
                    "AMB_Id": $scope.obj.AMB_Id,
                    "AMSE_Id": $scope.obj.AMSE_Id,
                    "ACMB_Id": $scope.obj.ACMB_Id,
                    "AMCST_Date": AMCST_Date,
                    "AMCST_DOB": AMCST_DOB,
                    "AMCST_DOBin_words": $scope.obj.AMCST_DOBin_words,
                    "IMCC_Id": $scope.obj.IMCC_Id,
                    "IMC_Id": $scope.obj.IMC_Id,
                    "AMCST_StudentSubCaste": $scope.obj.AMCST_StudentSubCaste,
                    "AMCST_StuBankIFSCCode": $scope.obj.AMCST_StuBankIFSCCode,
                    "AMCST_BirthPlace": $scope.obj.AMCST_BirthPlace,
                    "AMCST_BirthCertNo": $scope.obj.AMCST_BirthCertNo,
                    "AMCST_StuCasteCertiNo": $scope.obj.AMCST_StuCasteCertiNo,
                    "AMCST_Age": $scope.obj.AMCST_Age,
                    "AMCST_Sex": $scope.obj.AMCST_Sex,
                    "AMCST_MotherTongue": $scope.obj.AMCST_MotherTongue,
                    "IVRMMR_Id": $scope.obj.IVRMMR_Id,
                    "AMCST_StudentPhoto": $scope.obj.image,
                    "AMCST_Nationality": $scope.obj.AMCST_Nationality,
                    "Adm_College_Student_SMSNoDTO": $scope.mobilesstd,
                    "Adm_College_Student_EmailIdDTO": $scope.emailsstd,
                    "AMCST_AadharNo": $scope.obj.AMCST_AadharNo,
                    "AMCST_BloodGroup": $scope.obj.AMCST_BloodGroup,
                    "AMCOC_Id": $scope.obj.AMCOC_Id,
                    "ACQ_Id": $scope.obj.ACQ_Id,
                    "ACQC_Id": $scope.obj.ACQC_Id,
                    "ACSS_Id": $scope.obj.ACSS_Id,
                    "ACST_Id": $scope.obj.ACST_Id,
                    "AMCST_StuBankAccNo": $scope.obj.AMCST_StuBankAccNo,
                    "AMCST_BPLCardFlag": AMCST_BPLCardFlag,
                    "AMCST_BPLCardNo": $scope.obj.AMCST_BPLCardNo,
                    "AMCST_ECSFlag": AMCST_ECSFlag,
                    "AMCST_MobileNo": $scope.obj.amcsT_MobileNo,
                    "AMCST_emailId": $scope.obj.amcsT_emailId,
                    "transnumconfigsettings": $scope.RegistrationNumbering,
                    "admissionNumbering": $scope.AdmissionNumbering,
                    "AMCST_Taluk": $scope.obj.AMCST_Taluk,
                    "AMCST_Village": $scope.obj.AMCST_Village,
                    "AMCST_District": $scope.obj.AMCST_District,
                    "AMCST_Urban_Rural": $scope.obj.AMCST_Urban_Rural,
                    "AMCST_Divyangjan": $scope.obj.AMCST_Divyangjan
                    //  "AMST_Concession_Type": $scope.obj.feeconcession,

                    //  "AMST_SubCasteIMC_Id": $scope.obj.amsT_SubCasteIMC_Id,
                };

                if ($scope.obj.AMCST_ECSFlag === true) {
                    data.Adm_M_Student_ECS = $scope.ecsdetailslist;
                }

                apiService.create("CollegeStudentAdmission/saveStudentDetails", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.address = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.address = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Failed to Save/Update");
                        $scope.submitted1 = true;
                        $scope.address = true;

                    }
                });
            }
            else {
                $scope.submitted1 = true;
                $scope.address = true;
            }
        };


        //Second Tab
        $scope.savesecondtab = function () {
            if ($scope.myForm2.$valid) {
                var id = $scope.EditId;
                var data = {
                    "AMCST_Id": $scope.EditId,
                    "AMCST_PerStreet": $scope.obj.AMCST_PerStreet,
                    "AMCST_PerArea": $scope.obj.AMCST_PerArea,
                    "IVRMMC_Id": $scope.obj.IVRMMC_Id,
                    "AMCST_PerState": $scope.obj.AMCST_PerState,
                    "AMCST_PerCity": $scope.obj.AMCST_PerCity,
                    "AMCST_PerPincode": $scope.obj.AMCST_PerPincode,
                    "AMCST_ConStreet": $scope.obj.AMCST_ConStreet,
                    "AMCST_ConArea": $scope.obj.AMCST_ConArea,
                    "AMCST_ConCountryId": $scope.obj.AMCST_ConCountryId,
                    "AMCST_ConState": $scope.obj.AMCST_ConState,
                    "AMCST_ConCity": $scope.obj.AMCST_ConCity,
                    "AMCST_ConPincode": $scope.obj.AMCST_ConPincode,

                }
                apiService.create("CollegeStudentAdmission/saveAddress", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.Parents = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.Parents = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }

                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        swal("Failed to Save/Update");
                        $scope.submitted2 = true;
                        $scope.Parents = true;
                    }
                })
            }
            else {
                $scope.submitted2 = true;
                $scope.Parents = true;
            }
        }

        //Third Tab
        $scope.savethirdtab = function () {
            if ($scope.myForm3.$valid) {

                var id = $scope.EditId;
                var fathermobileno = $scope.mobiles;
                var fatheremailid = $scope.emails;
                var mothermobileno = $scope.mobiles1;
                var motheremailid = $scope.emails1;


                var data = {
                    "AMCST_Id": $scope.EditId,
                    "AMCST_FatherAliveFlag": $scope.obj.AMCST_FatherAliveFlag,
                    "AMCST_FatherName": $scope.obj.AMCST_FatherName,
                    "AMCST_FatherSurname": $scope.obj.AMCST_FatherSurname,
                    "AMCST_FatherAadharNo": $scope.obj.AMCST_FatherAadharNo,
                    "AMCST_FatherEducation": $scope.obj.AMCST_FatherEducation,
                    "AMCST_FatherOfficeAdd": $scope.obj.AMCST_FatherOfficeAdd,
                    "AMCST_FatherOccupation": $scope.obj.AMCST_FatherOccupation,
                    "AMCST_FatherDesignation": $scope.obj.AMCST_FatherDesignation,
                    "AMCST_FatherBankAccNo": $scope.obj.AMCST_FatherBankAccNo,
                    "AMCST_FatherBankIFSCCode": $scope.obj.AMCST_FatherBankIFSCCode,
                    "AMCST_FatherCasteCertiNo": $scope.obj.AMCST_FatherCasteCertiNo,
                    "AMCST_FatherNationality": $scope.obj.AMCST_FatherNationality,
                    "AMCST_FatherReligion": $scope.obj.AMCST_FatherReligion,
                    "AMCST_FatherCaste": $scope.obj.AMCST_FatherCaste,
                    "AMCST_FatherSubCaste": $scope.obj.AMCST_FatherSubCaste,
                    "AMCST_FatherMonIncome": $scope.obj.AMCST_FatherMonIncome,
                    "AMCST_FatherAnnIncome": $scope.obj.AMCST_FatherAnnIncome,
                    "AMCST_FatherMobleNo": $scope.obj.amcsT_FatherMobleNo,
                    "AMCST_FatheremailId": $scope.obj.amcsT_FatheremailId,

                    //Mother details
                    "AMCST_MotherAliveFlag": $scope.obj.AMCST_MotherAliveFlag,
                    "AMCST_MotherName": $scope.obj.AMCST_MotherName,
                    "AMCST_MotherSurname": $scope.obj.AMCST_MotherSurname,
                    "AMCST_MotherAadharNo": $scope.obj.AMCST_MotherAadharNo,
                    "AMCST_MotherEducation": $scope.obj.AMCST_MotherEducation,
                    "AMCST_MotherOfficeAdd": $scope.obj.AMCST_MotherOfficeAdd,
                    "AMCST_MotherOccupation": $scope.obj.AMCST_MotherOccupation,
                    "AMCST_MotherDesignation": $scope.obj.AMCST_MotherDesignation,
                    "AMCST_MotherBankAccNo": $scope.obj.AMCST_MotherBankAccNo,
                    "AMCST_MotherBankIFSCCode": $scope.obj.AMCST_MotherBankIFSCCode,
                    "AMCST_MotherCasteCertiNo": $scope.obj.AMCST_MotherCasteCertiNo,
                    "AMCST_MotherNationality": $scope.obj.AMCST_MotherNationality,
                    "AMCST_MotherReligion": $scope.obj.AMCST_MotherReligion,
                    "AMCST_MotherCaste": $scope.obj.AMCST_MotherCaste,
                    "AMCST_MotherSubCaste": $scope.obj.AMCST_MotherSubCaste,
                    "AMCST_MotherMonIncome": $scope.obj.AMCST_MotherMonIncome,
                    "AMCST_MotherAnnIncome": $scope.obj.AMCST_MotherAnnIncome,
                    "AMCST_MotherMobleNo": $scope.obj.amcsT_MotherMobleNo,
                    "AMCST_MotheremailId": $scope.obj.amcsT_MotheremailId,

                    "FatherMultipleMobileNoDTO": fathermobileno,
                    "FatherMultipleEmailIdDTO": fatheremailid,
                    "MotherMultipleMobileNoDTO": mothermobileno,
                    "MotherMultipleEmailIdDTO": motheremailid,
                }
                apiService.create("CollegeStudentAdmission/saveParentsDetails", data).then(function (promise) {
                    if (promise.message == "Add") {

                        swal("Record Saved Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.Others = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }
                    else if (promise.message == "Update") {
                        swal("Record Update Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.Others = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        $scope.submitted3 = true;
                        $scope.Others = true;

                    }
                })
            }

            else {
                $scope.submitted3 = true;
                $scope.Others = true;
            }
        }



        //Fourth Tab
        $scope.savefourthtab = function () {
            if ($scope.myForm4.$valid) {

                var id = $scope.EditId;
                //if ($scope.allActivity != "" && $scope.allActivity != null) {
                //    if ($scope.allActivity.length > 0) {
                //        for (var i = 0; i < $scope.allActivity.length; i++) {
                //            if ($scope.allActivity[i].Selected == true) {
                //                $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                //            }
                //        }
                //    }
                //}

                if ($scope.allRefrence != "" && $scope.allRefrence != null) {
                    if ($scope.allRefrence.length > 0) {
                        for (var i = 0; i < $scope.allRefrence.length; i++) {
                            if ($scope.allRefrence[i].Selected == true) {
                                $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                            }
                        }
                    }
                }

                if ($scope.allSources != "" && $scope.allSources != null) {
                    if ($scope.allSources.length > 0) {
                        for (var i = 0; i < $scope.allSources.length; i++) {
                            if ($scope.allSources[i].Selected == true) {
                                $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                            }
                        }
                    }
                }
                //
                //if ($scope.govtBondList != "" && $scope.govtBondList != null) {
                //    if ($scope.govtBondList.length > 0) {
                //        for (var i = 0; i < $scope.govtBondList.length; i++) {
                //            if ($scope.govtBondList[i].Selected == true) {
                //                $scope.SelectedGovtBonds.push($scope.govtBondList[i]);
                //            }
                //        }
                //    }
                //}



                // var ActivityIDs = $scope.Activitycheckboxchcked;
                var RefrenceIds = $scope.Refrencecheckboxchcked;
                var SourcesIds = $scope.Sourcescheckboxchcked;

                if ($scope.prevSchoolDetails.length > 0) {
                    angular.forEach($scope.prevSchoolDetails, function (pr) {
                        if (pr.acstpS_PrvSchoolName != "" && pr.acstpS_PrvSchoolName != null
                            && pr.acstpS_PrvSchoolName != undefined) {

                            if (pr.acstpS_PreviousTCDate !== undefined && pr.acstpS_PreviousTCDate !== null && pr.acstpS_PreviousTCDate !== "Invalid Date") {
                                pr.acstpS_PreviousTCDate = new Date(pr.acstpS_PreviousTCDate).toDateString();
                            } else {
                                pr.acstpS_PreviousTCDate = "";
                            }
                        }
                    });
                }

                var PrevSchoolDet = $scope.prevSchoolDetails;

                var StuGuardianDet = $scope.studentGuardianDetails;
                var StuSiblingDetails = $scope.studentSiblingDetails;

                var studentpreviousexammarksdetails = $scope.prevexammarksdetails;

                var AMCST_PassportIssueDate = "";
                var AMCST_PassportExpiryDate = "";
                var AMCST_VISAValidFrom = "";
                var AMCST_VISAValidTo = "";

                if ($scope.obj.AMCST_PassportNo != '' && $scope.obj.AMCST_PassportNo != null && $scope.obj.AMCST_PassportNo != undefined) {

                    AMCST_PassportIssueDate = $scope.obj.AMCST_PassportIssueDate;
                    AMCST_PassportExpiryDate = $scope.obj.AMCST_PassportExpiryDate;
                    
                } else {
                    AMCST_PassportIssueDate = new Date();
                    AMCST_PassportExpiryDate = new Date();
                    $scope.obj.AMCST_PassportNo = "";
                }
                if ($scope.obj.AMCST_VisaIssuedBy != '' && $scope.obj.AMCST_VisaIssuedBy != null && $scope.obj.AMCST_VisaIssuedBy != undefined) {
                    AMCST_VISAValidFrom = $scope.obj.AMCST_VISAValidFrom;
                    AMCST_VISAValidTo = $scope.obj.AMCST_VISAValidTo;
                } else {
                    AMCST_VISAValidFrom = new Date();
                    AMCST_VISAValidTo = new Date();
                    $scope.obj.AMCST_VisaIssuedBy = "";
                }

                var data = {

                    "AMCST_Id": $scope.EditId,
                    // "SelectedActivityDetails": ActivityIDs,
                    "SelectedRefrenceDetails": RefrenceIds,
                    "SelectedSourceDetails": SourcesIds,
                    // "SelectedAchivementDetails": $scope.obj.amsteC_Extracurricular,
                    // "SelectedBondDetails": $scope.SelectedGovtBonds,
                    "SelectedPrevSchoolDetails": PrevSchoolDet,
                    "SelectedSiblingDetails": StuSiblingDetails,
                    "SelectedGuardianDetails": StuGuardianDet,
                    "Adm_College_Student_SubjectMarksTempDTO": studentpreviousexammarksdetails,
                    "ACSTG_GuardianPhoto": $scope.acstG_GuardianPhoto,
                    "ACSTG_GuardianSign": $scope.acstG_GuardianSign,
                    "ACSTG_Fingerprint": $scope.acstG_Fingerprint,

                    "AMCST_PassportNo": $scope.obj.AMCST_PassportNo,
                    "AMCST_PassportIssuedAt": $scope.obj.AMCST_PassportIssuedAt,
                    "AMCST_PassportIssueDate": new Date(AMCST_PassportIssueDate).toDateString(),
                    "AMCST_PassportIssuedCounrty": $scope.obj.AMCST_PassportIssuedCounrty,
                    "AMCST_PassportIssuedPlace": $scope.obj.AMCST_PassportIssuedPlace,
                    "AMCST_PassportExpiryDate": new Date(AMCST_PassportExpiryDate).toDateString(),
                    "AMCST_VisaIssuedBy": $scope.obj.AMCST_VisaIssuedBy,
                    "AMCST_VISAValidFrom": new Date(AMCST_VISAValidFrom).toDateString(),
                    "AMCST_VISAValidTo": new Date(AMCST_VISAValidTo).toDateString()


                }
                apiService.create("CollegeStudentAdmission/saveOthersDetails", data).then(function (promise) {
                    if (promise.message == "Add") {

                        swal("Record Saved Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.DocumentUpload = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }

                    else if (promise.message == "Update") {

                        swal("Record Update Successfully");
                        $scope.EditId = promise.amcsT_Id;
                        $scope.DocumentUpload = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }

                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        $scope.submitted4 = true;
                        $scope.DocumentUpload = true;
                        swal("Failed to Save/Update Record");

                    }
                })
            }

            else {
                $scope.submitted4 = true;
                $scope.DocumentUpload = true;
            }
        }

        //Fifth Tab
        $scope.savefinaltab = function () {
            if ($scope.myForm5.$valid) {

                var data = {
                    "AMCST_Id": $scope.EditId,
                    "AMCST_FatherSign": $scope.fatherSign,
                    "AMCST_FatherFingerprint": $scope.fatherFingerprint,
                    "AMCST_MotherSign": $scope.mothersign,
                    "AMCST_MotherFingerprint": $scope.motherfingerprint,
                    "AMCST_FatherPhoto": $scope.fatherphoto,
                    "AMCST_MotherPhoto": $scope.motherphoto,
                    "AMCST_HostelReqdFlag": $scope.obj.AMCST_HostelReqdFlag,
                    "AMCST_TransportReqdFlag": $scope.obj.AMCST_TransportReqdFlag,
                    "AMCST_GymReqdFlag": $scope.obj.AMCST_GymReqdFlag,
                    Uploaded_documentList: $scope.documentList
                }
                apiService.create("CollegeStudentAdmission/saveDocuments", data).then(function (promise) {
                    if (promise.message == "Add") {

                        swal("Record Saved Successfully");
                        $state.reload();


                    }
                    else if (promise.message == "Update") {

                        swal("Record Update Successfully");
                        $state.reload();

                    }
                    else {
                        $scope.submitted4 = true;
                        $scope.disableSaveButton = false;
                        swal("Failed to Save/Update Record");
                    }
                })
            }
            else {
                $scope.submitted4 = true;
                $scope.disableSaveButton = false;
            }
        }


        //Clear Total Marks
        $scope.cleartotalmarks = function (prevSchool) {
            prevSchool.acstpS_PreviousMarksObtained = "";
            prevSchool.acstpS_PreviousPer = "";
        }

        //Percentage Calculation
        $scope.percentage_cal = function (prevSchool) {
            var total_marks = prevSchool.acstpS_PreviousMarks;
            var total_marksobtained = prevSchool.acstpS_PreviousMarksObtained;
            var percentage = (total_marksobtained / total_marks) * 100;

            if (percentage !== null && percentage !== "NaN") {
                prevSchool.acstpS_PreviousPer = parseFloat(percentage).toFixed(2);
            }
        };


        //Clear Total Marks
        $scope.clearobtainedmarks = function (prevExamdetails) {
            prevExamdetails.acstsuM_SubjectMarks = "";
            prevExamdetails.acstsuM_Percentage = "";
        };

        //Percentage Calculation
        $scope.percentage_calnew = function (prevExamdetails) {
            var total_marks = prevExamdetails.acstsuM_MaxMarks;
            var total_marksobtained = prevExamdetails.acstsuM_SubjectMarks;
            var percentage = (total_marksobtained / total_marks) * 100;

            if (percentage !== null && percentage !== "NaN") {
                prevExamdetails.acstsuM_Percentage = parseFloat(percentage).toFixed(2);
            }
        };

    }
})();
