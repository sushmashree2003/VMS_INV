
app.controller("appCtrl", ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', 'intracontroller', '$timeout', '$interval', '$sce',
    function ($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, intracontroller, $timeout, $interval, $sce) {

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        $scope.objupdate = {};
        var ip = "";
        $scope.iposbrowser = "";
        $.getJSON('https://ipinfo.io/json?token=a28739c42d2f25', function (data) {
            ip = data;
        });
        //if (navigator.serviceWorker) {
        //    navigator.serviceWorker.register('../service-worker.js', { scope: '/' })
        //        .then(function (registration) {
        //            console.log(registration);
        //        })
        //        .catch(function (e) {
        //            console.error(e);
        //        })
        //} else {
        //    console.log('Service Worker is not supported in this browser.')
        //}

        class DetectOS {
            constructor() {
                this.browser = this.searchString(this.dataBrowser())
                this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion)
                this.OS = this.searchString(this.dataOS())
            }

            searchString(data) {
                for (let i = 0; i < data.length; i++) {
                    let
                        dataString = data[i].string,
                        dataProp = data[i].prop
                    this.versionSearchString = data[i].versionSearch || data[i].identity
                    if (dataString) {
                        if (dataString.indexOf(data[i].subString) !== -1) {
                            return data[i].identity
                        }
                    } else if (dataProp) {
                        return data[i].identity
                    }
                }
            }

            searchVersion(dataString) {
                let index = dataString.indexOf(this.versionSearchString)
                if (index === -1) return
                return parseFloat(dataString.substring(index + this.versionSearchString.length + 1))
            }

            dataBrowser() {
                return [
                    /***************
                     * Chrome
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Chrome",
                        identity: "Chrome"
                    },
                    /***************
                     * Safari
                     ***************/
                    {
                        string: navigator.vendor,
                        subString: "Apple",
                        identity: "Safari",
                        versionSearch: "Version"
                    },
                    /***************
                     * For Older Opera (12.18-)
                     ***************/
                    {
                        prop: window.opera,
                        identity: "Opera",
                        versionSearch: "Version"
                    },
                    /***************
                     * Internet Explorer 10
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "MSIE",
                        identity: "IE10",
                        versionSearch: "MSIE"
                    },
                    /***************
                     * Internet Explorer 11
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Trident",
                        identity: "IE11",
                        versionSearch: "rv"
                    },
                    /***************
                     * Edge
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Edge",
                        identity: "Edge",
                        versionSearch: "Edge"
                    },
                    /***************
                     * Firefox
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Firefox",
                        identity: "Firefox"
                    },
                    {
                        string: navigator.userAgent,
                        subString: "Gecko",
                        identity: "Mozilla",
                        versionSearch: "rv"
                    },
                    /***************
                     * For Older Netscapes (4-)
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Mozilla",
                        identity: "Netscape",
                        versionSearch: "Mozilla"
                    },
                    /***************
                     * For Newer Netscapes (6+)
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Netscape",
                        identity: "Netscape"
                    },
                    /***************
                     * Other Browsers
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "OmniWeb",
                        versionSearch: "OmniWeb/",
                        identity: "OmniWeb"
                    },
                    {
                        string: navigator.vendor,
                        subString: "iCab",
                        identity: "iCab"
                    },
                    {
                        string: navigator.vendor,
                        subString: "KDE",
                        identity: "Konqueror"
                    },
                    {
                        string: navigator.vendor,
                        subString: "Camino",
                        identity: "Camino"
                    }
                ]
            }

            dataOS() {
                return [
                    {
                        string: navigator.platform,
                        subString: 'Win',
                        identity: 'Windows'
                    },
                    {
                        string: navigator.platform,
                        subString: 'Mac',
                        identity: 'macOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPhone',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPad',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPod',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'Android',
                        identity: 'Android'
                    },
                    {
                        string: navigator.platform,
                        subString: 'Linux',
                        identity: 'Linux'
                    }
                ]
            }
        }

        const Detect = new DetectOS()
        //document.addEventListener('contextmenu', event => event.preventDefault());
        //$(document).keydown(function (event) {
        //    if (event.keyCode == 123) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'I'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'J'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.keyCode == 'U'.charCodeAt(0)) {
        //        return false;
        //    }
        //});
        var promise;
        $scope.countdown = function () {
            // stops any running interval to avoid two intervals running at the same time
            $scope.stop();

            // store the interval promise
            promise = $interval(desktopnotification, 15000);
        };

        // stops the interval
        $scope.stop = function () {
            $timeout.cancel(promise);
            $interval.cancel(promise);
        };

        $scope.$on('$destroy', function () {
            $scope.stop();
        });

        $rootScope.theme = appSettings.theme;
        $rootScope.layout = appSettings.layout;
        $('#myModalswal').modal('hide');
        $scope.ProspectuseScreen = true;
        $scope.PaymentMode = false;
        $scope.reg = {};
        var configsettings = "";
        $scope.modulelistarry = {};
        $scope.pagelistarray = {};
        $scope.daspgenme = {};
        $scope.clickonmodules = function () {
            swal("Please use Left MenuBar to navigate and access Pages!!!");
        };
        var transnumconfigsettings1 = "";
        var vm = this;
        $scope.pageTitle = "Home";
        $scope.TaskNo = "";
        $scope.pageTitle = $cookieStore.get("pagetitle");

        $scope.ModuleName = $cookieStore.get("ModuleName");

        $scope.SubMenuName = $cookieStore.get("SubMenuName");
        //$scope.pageTitle = $cookieStore.get("pagetitle") ? $cookieStore.get("pagetitle") : "Home Page";
        $scope.setpagetitle = function (title) {
            $cookieStore.put("pagetitle", title);
            $scope.pageTitle = $cookieStore.get("pagetitle");
        };
        var dashboard = "";
        // set Module name
        $scope.setModuleName = function (title, moudleid) {
            if (title === "Dashboard") {
                $state.reload();
                $cookieStore.remove('SubMenuName');
                $cookieStore.remove('SubMenuName');
                $cookieStore.remove('pagetitle');

                $cookieStore.put("ModuleName", "Dashboard");
                $scope.ModuleName = $cookieStore.get("ModuleName");
                $scope.SubMenuName = $cookieStore.get("SubMenuName");
                $scope.pageTitle = $cookieStore.get("pagetitle");
            } else {

                $scope.moduleListsubmenu = $scope.modulelistarry;
                $cookieStore.put("ModuleName", title);
                $cookieStore.put("mouduleid", moudleid);
                $scope.ModuleName = $cookieStore.get("ModuleName");
            }
            $scope.modulefeedback(); //calling  Module FeedBack Mandatory Function
        }
        //For The Module FeedBack Mandatory Added By The Adarsh MS -- 22-05-2024
        $scope.modulefeedback = function () {
            $scope.modulefeedbackresult = "";
            $scope.ModuleName = $cookieStore.get("ModuleName");
            $scope.moudleid = $cookieStore.get("mouduleid");
            var info = {
                "ModuleName": $scope.ModuleName,
                "IVRMM_Id": $scope.moudleid
            }
            apiService.create("ModuleFeedback/checkfeedback", info).then(function (response) {
                $scope.modulefeedbackresult = response.nofeedbackfound;
                if ($scope.modulefeedbackresult == "notfound") {
                    // swal("Please Give The FeedBack Of The" + " "+ $scope.ModuleName + " "+"Module To Access")3
                    swal({
                        title: "Current Month Feedback Not Given For" + " " + $scope.ModuleName,
                        text: "You will not be able to access module! Kindly give Feedback",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Give FeedBack",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function () {
                            window.location.href = "http://localhost:57606/#/app/ModuleFeedback/";
                        });
                }
            })
        }
        //END OF MODULE FEEDBACK


        var HostName = location.host;
        $scope.fillpay = function (pasR_Id) {
            $window.location.href = 'https://' + HostName + '/#/app/PreadmissionOnlinePayment/';
        };
        //SubMenuName
        $scope.setSubMenuName = function (title, menuId, moduleId) {
            if ($scope.modulefeedbackresult == "notfound") {
                $scope.modulefeedback();
            } else {
                $cookieStore.put("SubMenuName", title);
                $scope.SubMenuName = $cookieStore.get("SubMenuName");
                $scope.pageListT = [];
                $scope.pageList = [];
                $scope.pageListM = [];
                angular.forEach($scope.pagelistarray, function (opqr1) {
                    if (opqr1.menuId === menuId) {
                        $scope.pageListT.push(opqr1);
                    }
                });
                angular.forEach($scope.pagelistarray, function (opqr1) {
                    if (opqr1.moduleId == moduleId) {
                        $scope.pageListM.push(opqr1);
                    }
                })

                if ($scope.pageListT.length >= 10) {
                    $scope.pageList = $scope.pageListT;
                }
                else if ($scope.pageListT.length >= 1 && $scope.pageListT.length <= 5) {
                    angular.forEach($scope.pagelistarray, function (opqr1) {
                        if ($scope.pageListM.length >= 100) {
                            if (opqr1.menuId == menuId) {
                                $scope.pageList.push(opqr1);
                            }
                        }
                        else {
                            if (opqr1.moduleId == moduleId) {
                                $scope.pageList.push(opqr1);
                            }
                        }
                    });
                }
                else {
                    angular.forEach($scope.pagelistarray, function (opqr1) {
                        if (opqr1.menuId == menuId) {
                            $scope.pageList.push(opqr1);
                        }
                    });
                }
            }


        }
        $scope.fetchhomepage = function () {
            var data = {
            };
            apiService.create("Login/getrolewisepage", data).then(function (promise) {

                if (promise.filldashpagemap !== null && promise.filldashpagemap.length > 0) {
                    if (promise.pageexists.length > 0) {

                        $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName;
                    }
                    else {
                        $scope.daspgenme = "app.homepage";
                    }
                }
                else {
                    $scope.daspgenme = "app.homepage";
                }
                $state.go($scope.daspgenme);
            });
        };
        //avalilable themes
        vm.themes = [
            {
                theme: "black",
                color: "skin-black",
                title: "Dark - Black Skin",
                icon: ""
            },
            {
                theme: "black",
                color: "skin-black-light",
                title: "Light - Black Skin",
                icon: "-o"
            },
            {
                theme: "blue",
                color: "skin-blue",
                title: "Dark - Blue Skin",
                icon: ""
            },
            {
                theme: "blue",
                color: "skin-blue-light",
                title: "Light - Blue Skin",
                icon: "-o"
            },
            {
                theme: "green",
                color: "skin-green",
                title: "Dark - Green Skin",
                icon: ""
            },
            {
                theme: "green",
                color: "skin-green-light",
                title: "Light - Green Skin",
                icon: "-o"
            },
            {
                theme: "yellow",
                color: "skin-yellow",
                title: "Dark - Yellow Skin",
                icon: ""
            },
            {
                theme: "yellow",
                color: "skin-yellow-light",
                title: "Light - Yellow Skin",
                icon: "-o"
            },
            {
                theme: "red",
                color: "skin-red",
                title: "Dark - Red Skin",
                icon: ""
            },
            {
                theme: "red",
                color: "skin-red-light",
                title: "Light - Red Skin",
                icon: "-o"
            },
            {
                theme: "purple",
                color: "skin-purple",
                title: "Dark - Purple Skin",
                icon: ""
            },
            {
                theme: "purple",
                color: "skin-purple-light",
                title: "Light - Purple Skin",
                icon: "-o"
            }
        ];
        //status progress bar
        $scope.Status = [
            {
                title: "Complete Status",
                theme: "red",
                percentage: 80
            }
        ];
        //available layouts
        vm.layouts = [
            {
                name: "Boxed",
                layout: "layout-boxed"
            },
            {
                name: "Fixed",
                layout: "fixed"
            },
            {
                name: "Sidebar Collapse",
                layout: "sidebar-collapse"
            }
        ];
        //set the theme selected
        vm.setTheme = function (value) {
            $rootScope.theme = value;
        };
        //set the Layout in normal view
        vm.setLayout = function (value) {
            $rootScope.layout = value;
        };
        //controll sidebar open & close in mobile and normal view
        vm.sideBar = function (value) {
            if ($(window).width() <= 1500) {
                if ($("body").hasClass('sidebar-open'))
                    $("body").removeClass('sidebar-open');
                else
                    $("body").addClass('sidebar-open');
            }
            else {
                if (value === 1) {
                    if ($("body").hasClass('sidebar-collapse'))
                        $("body").removeClass('sidebar-collapse');
                    else
                        $("body").addClass('sidebar-collapse');
                }
            }
        };
        //navigate to search page
        vm.search = function () {
            $state.go('app.search');
        };
        //$scope.roleType = function () {
        //    apiService.get("Login/getRole/").
        //   then(function (promise) {
        //       if (promise != "") {
        //           alert(promise);
        //           return promise;
        //           //$scope.vm.roleId = promise;
        //           //$state.go('app.homepage');
        //       }
        //   });
        //}
        // var HostName = location.host;
        $scope.fillapp = function (pasR_Id) {
            var data = {
                "pasR_Id": pasR_Id
            };
            apiService.create("StudentApplication/getdashboardpage", data).then(function (promise) {

                $window.location.href = 'https://' + HostName + '/#/app/' + promise.dashboardpage + '/';

            });
        }
        $scope.GetRoleTypesnothing = function () {
            var data = {
                "pasR_Id": 1
            };
            apiService.create("Login/GetRoleTypesnothing", data).then(function (promise) {
                // $window.location.href = 'https://' + HostName + '/#/app/' + promise.dashboardpage + '/';
            });
        };
        $scope.GetRoleTypes = function () {
            $cookieStore.remove('SubMenuName');
            $cookieStore.remove('pagetitle');
            $cookieStore.put("SubMenuName", "");
            $cookieStore.put("pagetitle", "");
            
            $cookieStore.put("ModuleName", "Dashboard");
            $scope.TaskNo = "";
            apiService.get("Login/getRole/").then(function (promise) {
                if (promise !== "" && promise.isSessionExpired != true) {
                    $scope.notifycount = promise.notifycount;
                    $scope.userauth = promise.twoFactorEnabled;
                    //$('#toggle-trigger').bootstrapToggle('on')
                    $scope.totalfeed = promise.totalfeed;
                    console.log(promise.filldata);
                    $scope.moduleList = promise.moduleList;
                    $scope.moduleListonly = promise.moduleListonly;
                    $scope.modulelistarry = promise.moduleList;
                    $scope.modulepagecategoryList = promise.moduleCategoryList;
                    $scope.pagelistarray = promise.pageList;
                    $scope.dashboardpgemap = promise.filldashpagemap;

                    if (promise.filldashpagemap != undefined && promise.filldashpagemap !== null && promise.filldashpagemap.length > 0) {
                        $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName;
                    }
                    else {
                        $scope.daspgenme = "app.homepage";
                    }

                    if (promise.studname != null) {
                        $scope.ufullName = promise.studname;
                        $scope.halfname = promise.studnamehalf;
                        $scope.halflength = promise.studnamehalf.length;
                        $scope.userlength = promise.studname.length;
                    }
                    else {
                        $scope.ufullName = null;
                    }
                    if (promise.userData != undefined && promise.userData != null && promise.userData.length > 0) {
                        $scope.uName = promise.userData[0].userName;
                        localStorage.setItem("username", $scope.uName);
                        $scope.uName = promise.userData[0].userName;
                        $scope.rName = promise.userData[0].roleName;
                        $scope.imgname = promise.userData[0].userImagePath;
                        $scope.Userappname = promise.userData[0].userappname;
                        $scope.Useremail = promise.userData[0].useremail;
                        $scope.usermob = promise.userData[0].usermob;
                    }
                    $scope.midata = promise.mIdata;
                    var list = promise.privileges;

                    if ($scope.rName == 'OnlinePreadmissionUser') {
                        $scope.adminmodel = false;
                        $scope.usermodel = true;
                        $scope.admissionstudent = false;
                    }
                    else if ($scope.rName == 'HR') {
                        if (promise.count_d == 1) {
                            $scope.update = false;
                            $scope.grid_view = false;
                            $('#myModalAbsent').modal('hide');
                        }
                        else {
                            $('#myModalAbsent').modal('show');

                            if (promise.filldata.length > 0) {
                                $scope.grid_view = true;
                                $scope.yearlyemprep = promise.filldata;
                                $scope.no_planner_creation = promise.no_planner_creation;
                                $scope.prestartdate = promise.cutoffdate;
                                $scope.lastdate = promise.dob;

                                $scope.yearlyemprep = promise.filldata;
                                $scope.totalworkingdays = promise.totalworkingdays;
                                $scope.todate_date = new Date;
                                $scope.temp_list = [];
                                for (var i = 0; i < $scope.yearlyemprep.length; i++) {
                                    var totalAbsent = 0;
                                    totalAbsent = $scope.yearlyemprep[i].absentdays;
                                    if (totalAbsent > 0) {
                                        $scope.temp_list.push({

                                            "ecode": $scope.yearlyemprep[i].ecode, "ename": $scope.yearlyemprep[i].ename,
                                            "HRMDES_DesignationName": $scope.yearlyemprep[i].HRMDES_DesignationName, "HRMD_DepartmentName": $scope.yearlyemprep[i].DeptName, "NoOfAbsent": $scope.yearlyemprep[i].absentdays, "hrme_id": $scope.yearlyemprep[i].hrme_id
                                        });
                                    }
                                }
                                $scope.chktwd1 = false;
                                $scope.chkjoin1 = false;
                                $scope.chkhwk1 = false;
                                if ($scope.chktwd == true) {
                                    $scope.chktwd1 = true;
                                    $scope.columnnum = $scope.columnnum + 1;
                                }
                                if ($scope.chkjoin == true) {
                                    $scope.chkjoin1 = true;
                                    $scope.columnnum = $scope.columnnum + 1;
                                }
                                if ($scope.chkhwk == true) {
                                    $scope.chkhwk1 = true;
                                    $scope.columnnum = $scope.columnnum + 1;
                                }
                                console.log($scope.temp_list);
                            }
                        }
                    }
                    else if ($scope.rName == 'STUDENT') {
                        $scope.adminmodel = false;
                        $scope.usermodel = false;
                        $scope.admissionstudent = true;
                    }
                    else {
                        $scope.adminmodel = true;
                        $scope.usermodel = false;
                        $scope.admissionstudent = false;
                    }


                    localStorage.setItem("privileges", JSON.stringify(list));

                    var conlistlist = promise.configlist;
                    localStorage.setItem("configsettings", JSON.stringify(conlistlist));

                    if (conlistlist != null && conlistlist.length > 0) {
                        $scope.configurationsettings = conlistlist[0];
                    }

                    var transnumconfig = promise.transnumconfig;
                    localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));

                    transnumconfigsettings1 = transnumconfig;

                    var feeconlistlist = promise.feeconfiglist;
                    localStorage.setItem("feeconfigsettings", JSON.stringify(feeconlistlist));

                    var admlistlist = promise.admissioncongigurationList;
                    localStorage.setItem("admfigsettings", JSON.stringify(admlistlist));

                    var academicyearlist = promise.fillyear;
                    localStorage.setItem("academicyearlist", JSON.stringify(academicyearlist));

                    var ivrmgeneralconfiglist = promise.ivrmconfiglist;
                    localStorage.setItem("ivrmgeneralconfiglist", JSON.stringify(ivrmgeneralconfiglist));

                    var ivrmgeneralhomepagelist = promise.filldashpagemap;
                    localStorage.setItem("ivrmgeneralhomepagelist", JSON.stringify(ivrmgeneralhomepagelist));

                    var ivrmstoragelist = promise.storagedetails;
                    localStorage.setItem("ivrmstoragelist", JSON.stringify(ivrmstoragelist));

                    if (promise.fillinstition != undefined && promise.fillinstition != null && promise.fillinstition.length > 0) {
                        $scope.institutionname = promise.fillinstition[0].mI_Code;
                    }

                    if ($scope.rName == 'STUDENT') {
                        //$scope.admno = promise.studentdata[0].admno;
                        //$scope.clsname = promise.studentdata[0].clsnme;
                        //$scope.secname = promise.studentdata[0].secnme;
                        //$scope.dobirth = promise.studentdata[0].dob;
                        //$scope.emailid = promise.studentdata[0].emailid;
                        //$scope.mobno = promise.studentdata[0].mobno;

                        $scope.imgsrc = promise.studentdata[0].imgnme;
                        $scope.imgname = promise.studentdata[0].imgnme;

                        //$scope.studentname = promise.studentdata[0].studname;
                        if (promise.userImagePath !== "" && promise.userImagePath != null) {
                            $scope.imgsrc = promise.userImagePath;
                            $scope.imgname = promise.userImagePath;
                        }

                        var mandatory = promise.manadatoryfields;
                        localStorage.setItem("fieldsManadatory", JSON.stringify(mandatory));
                        //$scope.countdown();
                    }
                    var mandatory = promise.manadatoryfields;
                    localStorage.setItem("fieldsManadatory", JSON.stringify(mandatory));

                }
                else {
                    window.location.href = "http://localhost:57606/#/login/";
                }
                //$scope.regformfour();
            });
           
            $scope.remainmelater = $cookieStore.get("remainmelater");
            if ($scope.remainmelater !="true") {
              //  $scope.silverjubliee_suggestion_show();
            }
           

        };


        $scope.submittedsilver = false;
        $scope.silver = {};
       
        const forbiddenWords = [
            "good", "super", "nothing", "na", ".", ",", "no suggestion",
            "no suggestions", "no", "excellent", "its good",
            "the silver celebration is very good", "bad", "average", "ok", "okay", "better", "   ", "  ", "nan", "Fine", "Not Applicable", "Thank you", "Better",
            "next Time", "Remind Me later", "No Issues", "done", "below average", "bad", "worst","its good","happy to work"];

        $scope.errorMessage = false;

        // Function to check forbidden words and phrases
        $scope.checkForbiddenWords = function () {
            if ($scope.silver && $scope.silver.silverjubileesuggestion) {
                const inputText = $scope.silver.silverjubileesuggestion.trim().toLowerCase(); // Convert to lowercase for case-insensitive matching

                // Check if the input is an exact match with any forbidden word or phrase
                $scope.errorMessage = forbiddenWords.some(function (forbidden) {
                    // Remove any punctuation from the input and forbidden phrase
                    const sanitizedInput = inputText.replace(/[.,/#!$%^&*;:{}=\-_`~()]/g, "").trim();
                    const sanitizedForbidden = forbidden.replace(/[.,/#!$%^&*;:{}=\-_`~()]/g, "").trim();

                    // Check for exact match (case-insensitive)
                    return sanitizedInput === sanitizedForbidden;
                });
            }
        };

        // to get the silver jubilee feedback
        $scope.silverjubliee_suggestion_show = function () {
            
                var info = {
                    "nofeedbackfound": "silerjubilee"
                }
                apiService.create("ModuleFeedback/checkfeedback", info).then(function (response) {
                    $scope.existfeedback = response.existfeedback;
                    if ($scope.existfeedback != "found" ) {
                        $('#Silverjubileesuggestion').modal('show');
                    }
                })
            

        }
        $scope.silverjubliee_suggestion_save = function () {
            if ($scope.myForm_suggestion.$valid && $scope.errorMessage == false) {
                var info = {
                    "nofeedbackfound": "silerjubilee",
                    "feedbacktask": $scope.silver.silverjubileesuggestion,
                    "VMF_Rating": $scope.rating

                }
                apiService.create("ModuleFeedback/submitfeedback", info).then(function (response) {
                    $scope.existfeedback = response.existfeedback;
                    if ($scope.existfeedback == "saved") {
                        swal("Suggestion Captured Successfully", "Thank You", "success");
                        $('#Silverjubileesuggestion').modal('hide');
                    }
                })
            }
            else {
                $scope.submittedsilver = true;
            }
        }
        $scope.closesilverjubilee = function () {
            $('#Silverjubileesuggestion').modal('hide');

            $cookieStore.put("remainmelater", "true");
        }
        $scope.interacted_silverjubileesuggestion = function (field) {
            return $scope.submittedsilver;
        };
        // to get the silver jubilee feedback END


        console.log('getting in to the app controller');
        vm.logout = function () {
            $cookieStore.remove('pagetitle');
            $cookieStore.put("pagetitle", "Home");
            $cookieStore.remove('IsLogged');
            $cookieStore.put("remainmelater","false");
            apiService.getURI("Login/clearsession", 1).
                then(function (promise) {
                    //$state.go("login");
                    $scope.stop();
                    if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                        window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                    }
                    else {
                        window.location.href = "http://localhost:57606/#/login/";
                    }

                });
        };

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            //  $state.reload();


        }

        vm.InstituteChange = function (mI_Id) {

            var id = mI_Id;

            $cookieStore.remove('SubMenuName');
            $cookieStore.remove('pagetitle');
            $cookieStore.put("SubMenuName", "");
            $cookieStore.put("pagetitle", "");

            $cookieStore.put("ModuleName", "Dashboard");

            apiService.getURI("Login/getRoleoForChangeInstitute/", id).
                then(function (promise) {
                    $scope.GetRoleTypes();
                    $state.reload();
                });
        };

        vm.institute = function () {
            apiService.getURI("Login/getMIdataMaster", 0).then(function (promise) {

                $scope.viewpopup = "false";

                $scope.MIdata = promise.mIdata;

                if (promise.mIdata.length > 1) {
                    $('#institution').modal('show');
                }
                else if (promise.mIdata.length == 1) {
                    $scope.GetRoleTypes();
                }
                else {
                    $scope.institution = '#';
                }
            });
        };

        //**************************************************************** ISM Interaction ************************************************************//
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings !== null) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    if (transnumconfigsettings[i].imN_Flag === "ISMInteraction") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }
        }
        $scope.obj = {};
        $scope.inboxTab = true;
        $scope.newmessageTab = false;
        $scope.oninboxtab = function () {
            $scope.newmessageTab = false;
            $scope.inboxTab = true;
        };
        $scope.onnewmessagetab = function () {
            $scope.inboxTab = false;
            $scope.newmessageTab = true;
        };
        $scope.onintercationclick = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 20;
            var pageid = 2;
            apiService.getURI("ISM_Interaction/getdetails", pageid).then(function (promise) {
                $scope.roletype = promise.roletype;
                $scope.get_toEmplist = promise.get_toEmplist;
                $scope.employee_Id = promise.employee_Id;
                if (promise.get_toEmplist.length > 0) {
                    $('#myModalInteraction').modal('show');
                }
                $scope.get_inboxMessages = promise.get_inboxMessages;
                $scope.inboxtotal = $scope.get_inboxMessages.length;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        //=========================Feedback Details===============
        $scope.notifyclick = function () {
            $scope.currentPage22 = 1;
            $scope.itemsPerPage22 = 4;
            var pageid = 2;
            $scope.get_feedbackdetails = [];
            $scope.notificationlist = [];
            apiService.getURI("ISM_Interaction/getnotification", pageid).then(function (promise) {

                $('#myModalNotify').modal('show');
                $scope.notificationlist = promise.get_feedbacklist;

            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };



        $scope.feedbackclick = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 20;
            var pageid = 2;
            $scope.get_feedbackdetails = [];
            $scope.get_feedbacklist = [];
            apiService.getURI("ISM_Interaction/getfeedbac", pageid).then(function (promise) {
                //$scope.roletype = promise.roletype;
                //$scope.get_toEmplist = promise.get_toEmplist;
                // $scope.employee_Id = promise.employee_Id;
                //if (promise.inbcount.length > 0) {
                //    $('#myModalfeedback').modal('show');
                //}
                $('#myModalfeedback').modal('show');
                $scope.get_feedbacklist = promise.get_feedbacklist;
                $scope.feedcount = promise.feedbackcount.length;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        $scope.clicknotification = function (msg) {
            if (msg.ISMNO_ReadFlg == true) {
                if (msg.ISMNO_RedirectURL != null && msg.ISMNO_RedirectURL != '' && msg.ISMNO_RedirectURL != undefined) {
                    $window.open(msg.ISMNO_RedirectURL, '_blank');
                    //intracontroller.taskno = "TASK/17498/2020-2021";
                    //intracontroller.ISMTCR_Id = 124550;
                    //var ISMNO_RedirectURL = 'http://' + HostName + '/#/app/ISM_ComplaintsTaskList/124550';
                    //$('#myModalNotify').modal('hide');
                    //$window.open(msg.ISMNO_RedirectURL, '_parent');
                }
            }
            else {
                var data = {
                    "ISMNO_Id": msg.ISMNO_Id
                };
                apiService.create("ISM_Interaction/clicknotification", data).then(function (promise) {
                    //$('#myModalNotify').modal('hide');
                    $scope.notificationlist = promise.get_feedbacklist;
                    $scope.notifycount = promise.notifycount;
                    if (msg.ISMNO_RedirectURL != null && msg.ISMNO_RedirectURL != '' && msg.ISMNO_RedirectURL != undefined) {
                        $window.open(msg.ISMNO_RedirectURL, '_blank');
                    }

                });
            }


        };

        $scope.feedbackdetailsclick = function (msg) {
            $scope.showreply = false;
            var data = {
                "ISMDRF_Send_HRME_Id": msg.ISMDRF_Send_HRME_Id
                // "ISMDRF_Id": msg.ISMDRF_Id
            };
            apiService.create("ISM_Interaction/feedback_msg_details", data).then(function (promise) {
                $scope.get_feedbackdetails = promise.get_feedbackdetails;
                $scope.sender_empname = promise.from_emp_details[0].sender_empname;
                $scope.hrmD_DepartmentName = promise.from_emp_details[0].hrmD_DepartmentName;
                $scope.get_feedbacklist = promise.get_feedbacklist;
                $scope.feedcount = promise.inbcount;
                $scope.totalfeed = promise.inbcount;
            });
        };

        function desktopnotification() {
            var data = {
                "ISMDRF_Send_HRME_Id": 111
            };
            apiService.create("ISM_Interaction/desktopnotification", data).then(function (promise) {
                $scope.notifycount = promise.notifycount;
            });
        }

        $scope.Sendfeedback = function (plnr) {
            $scope.feedbackdtl = [];
            angular.forEach($scope.get_feedbackdetails, function (ss) {
                if (ss.feedbact !== undefined && ss.feedbact !== null && ss.feedbact !== "") {
                    $scope.feedbackdtl.push({
                        ISMDRF_RCV_HRME_Id: ss.ISMDRF_Send_HRME_Id, ISMTCR_Id: ss.ISMTCR_Id,
                        ISMDRF_FeedBack: ss.feedbact, MI_Id_r: ss.MI_Id, ISMDRF_Feedback_DR_Date: ss.ISMDRF_Feedback_DR_Date,
                        ISMDRF_OpenFeedback: ss.ISMDRF_OpenFeedback, ISMDRPT_Date: ss.ISMDRPT_Date, ISMDRF_Id: ss.ISMDRF_Id
                    });
                }
            });

            if ($scope.feedbackdtl !== null && $scope.feedbackdtl.length > 0) {

                var data = {
                    feedbackdtl: $scope.feedbackdtl
                };
                apiService.create("ISM_Interaction/Sendfeedback_tfr", data).then(function (promise) {
                    if (promise.f_return === "Success") {
                        $scope.temp = [];
                        $scope.temp = $scope.get_feedbackdetails;
                        swal("Feedback Sent Successfully...");
                        $state.reload();
                    }
                    else {
                        swal("Feedback Not sent successfully...");
                        $scope.feedbact = "";
                    }
                });
            } else {
                swal("Enter Feedback");
            }
        };

        $scope.getViewfeedback = function (qqq) {
            var data = {
                "ISMTCR_Id": qqq.ISMTCR_Id
            };
            apiService.create("ISM_Interaction/getViewfeedback", data).then(function (promise) {
                $scope.get_feedback = promise.get_feedback;
            });
        };
        //================================feedback details end================================
        //*********************************** To All Check Box Selection & Validation
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_toEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_toEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.isOptionsRequiredINT = function () {
            if ($scope.obj.groupOrIndFlg === "G") {
                return !$scope.get_toEmplist.some(function (options) {
                    return options.empck;
                });
            }
        };
        //*********************************** Send Message / Save Data
        $scope.savedata = function () {
            $scope.submitted1 = true;
            var data = {};
            if ($scope.myFormInteraction.$valid) {
                $scope.arraymessage = [];
                angular.forEach($scope.get_toEmplist, function (emp) {
                    if (emp.empck === true) {
                        $scope.arraymessage.push(emp);
                    }
                });
                if ($scope.obj.groupOrIndFlg === "G") {
                    data = {
                        "roletype": $scope.roletype,
                        "ISMINTR_GroupOrIndFlg": $scope.obj.groupOrIndFlg,
                        "ISMINTR_Subject": $scope.obj.ismintR_Subject,
                        "ISMINTR_Interaction": $scope.obj.ismintR_Interaction,
                        "arraymessage": $scope.arraymessage
                    };
                }
                else {
                    data = {
                        "roletype": $scope.roletype,
                        "ISMINTR_GroupOrIndFlg": $scope.obj.groupOrIndFlg,
                        "ISMINTR_Subject": $scope.obj.ismintR_Subject,
                        "ISMINTR_Interaction": $scope.obj.ismintR_Interaction,
                        "ISMINTRD_ToId": $scope.obj.hrmE_Id.HRME_Id
                    };
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ISM_Interaction/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismintR_Id === 0 || promise.ismintR_Id < 0) {
                            swal('Message sent successfully');
                            $('.modal-backdrop').remove();
                            $state.reload();
                        }
                    }
                    else {
                        if (promise.ismintR_Id === 0 || promise.ismintR_Id < 0) {
                            swal('Failed to send, please contact administrator');
                        }
                        else if (promise.ismintR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $('.modal-backdrop').remove();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        //***********************************  Message Details
        $scope.onmessageclick = function (msg) {
            $scope.msgroup = [];
            $scope.replyto = [];
            $scope.toname = "";
            $scope.showreply = false;
            $scope.get_messageDetail = [];
            $scope.Inter_Id = msg.ISMINTR_Id;
            var data = {
                "ISMINTR_Id": $scope.Inter_Id
            };
            apiService.create("ISM_Interaction/get_replydetails", data).
                then(function (promise) {
                    $scope.get_messageDetail = promise.get_messageDetail;
                    if ($scope.get_messageDetail.length > 0) {
                        $scope.msg_id = $scope.get_messageDetail[0].ISMINTR_Id;
                        $scope.employeename = $scope.get_messageDetail[0].employeename;
                        $scope.Sender = $scope.get_messageDetail[0].Sender;
                        $scope.msg_subject = $scope.get_messageDetail[0].ISMINTR_Subject;
                        $scope.msg_message = $scope.get_messageDetail[0].ISMINTR_Interaction;
                        $scope.msg_DateTime = $scope.get_messageDetail[0].ISMINTR_DateTime;
                        $scope.msg_type = $scope.get_messageDetail[0].ISMINTR_GroupOrIndFlg;
                        $scope.msg_user = $scope.get_messageDetail[0].user_name;
                        $scope.messageflg = $scope.get_messageDetail[0].messageflg;
                        if ($scope.msg_type === "G") {
                            angular.forEach($scope.get_messageDetail, function (msgd, index) {
                                $scope.msgroup.push({ ISMINTR_Id: msgd.ISMINTR_Id, ISMINTRD_ToId: msgd.ISMINTRD_ToId, Sender: msgd.Sender, Receiver: msgd.Receiver, ISMINTR_GroupOrIndFlg: msgd.ISMINTR_GroupOrIndFlg, messageflg: msgd.messageflg });
                                if (index === 0) {
                                    $scope.toname = msgd.Receiver;
                                }
                                else {
                                    $scope.toname = $scope.toname + ' , ' + msgd.Receiver;
                                }
                            });
                        }
                        else {
                            $scope.msgroup.push({ ISMINTR_Id: $scope.get_messageDetail[0].ISMINTR_Id, ISMINTRD_ToId: $scope.get_messageDetail[0].ISMINTRD_ToId, Receiver: $scope.get_messageDetail[0].Receiver, ISMINTR_GroupOrIndFlg: $scope.get_messageDetail[0].ISMINTR_GroupOrIndFlg, messageflg: $scope.get_messageDetail[0].messageflg });
                            $scope.toname = $scope.get_messageDetail[0].Receiver;
                        }
                    }
                    $scope.get_replyMessages = promise.get_replyMessages;
                });
        };
        //***********************************  Reply     
        $scope.showreply = false;
        $scope.replyclick = function (msgdata) {
            $scope.replytoname = "";
            $scope.msgroup = [];
            $scope.obj.ismintrD_Interaction = [];
            $(".panel-collapse").collapse("hide");
            $scope.showreply = true;
            if (msgdata.length > 0) {
                $scope.msg_id = msgdata[0].ISMINTR_Id;
                $scope.msg_type = msgdata[0].ISMINTR_GroupOrIndFlg;
                $scope.messageflg = msgdata[0].messageflg;
                $scope.ISMINTRD_ToId = $scope.get_messageDetail[0].ISMINTR_ComposedByHRME_Id;
                $scope.Senderuser = $scope.get_messageDetail[0].Sender;
                if ($scope.msg_type === "G") {

                    if ($scope.employee_Id === $scope.get_messageDetail[0].ISMINTR_ComposedByHRME_Id) {
                        angular.forEach($scope.get_messageDetail, function (msgd, index) {
                            $scope.msgroup.push({ ISMINTR_Id: msgd.ISMINTR_Id, ISMINTRD_ToId: msgd.ISMINTRD_ToId, Sender: msgd.Sender, Receiver: msgd.Receiver, ISMINTR_GroupOrIndFlg: msgd.ISMINTR_GroupOrIndFlg, messageflg: msgd.messageflg });
                            if (index === 0) {
                                $scope.replytoname = msgd.Receiver;
                            }
                            else {
                                $scope.replytoname = $scope.replytoname + ' , ' + msgd.Receiver;
                            }
                        });
                    }
                    else {
                        angular.forEach($scope.get_messageDetail, function (msgd, index) {
                            $scope.msgroup.push({ ISMINTR_Id: msgd.ISMINTR_Id, ISMINTRD_ToId: msgd.ISMINTR_ComposedByHRME_Id, Sender: msgd.Sender, Receiver: msgd.Receiver, ISMINTR_GroupOrIndFlg: msgd.ISMINTR_GroupOrIndFlg, messageflg: msgd.messageflg });
                            if (index === 0) {
                                $scope.replytoname = msgd.Sender + ' , ' + msgd.Receiver;
                            }
                            else {
                                $scope.replytoname = $scope.replytoname + ' , ' + msgd.Receiver;
                            }
                        });
                    }
                }
                else {
                    $scope.msgroup.push({ ISMINTR_Id: $scope.get_messageDetail[0].ISMINTR_Id, ISMINTRD_ToId: $scope.get_messageDetail[0].ISMINTR_ComposedByHRME_Id, Receiver: $scope.get_messageDetail[0].Receiver, ISMINTR_GroupOrIndFlg: $scope.get_messageDetail[0].ISMINTR_GroupOrIndFlg, messageflg: $scope.get_messageDetail[0].messageflg });
                    $scope.replytoname = $scope.get_messageDetail[0].Sender;
                }
            }
        };
        //==================== save data=======
        $scope.savedata3 = function () {
            $('#save_ack').modal('show');
        };
        $scope.save_click = function () {
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {
                data = {
                    "ISM_AS_Remarks": $scope.ISM_AS_Remarks
                };
                apiService.create("MasterStatus/save_ack", data).then(function (promise) {
                    if (promise.returnval == true) {
                        $('#myModalAbsent').modal('hide');
                        $scope.update = true;
                        swal("Update Successfully");
                        $state.reload();
                    }
                });
            }
            else {

                $scope.submitted = true;
            }
        }
        //*********************************** Send Reply 
        $scope.saveReply = function (msgid) {
            var data = {};
            $scope.ISMINTR_Id = msgid;
            $scope.arrayreply = [];

            data = {
                "ISMINTR_Id": $scope.ISMINTR_Id,
                "ISMINTRD_Interaction": $scope.obj.ismintrD_Interaction,
                "ISMINTRD_ToId": $scope.ISMINTRD_ToId,
                "arrayreply": $scope.msgroup
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ISM_Interaction/savereply", data).then(function (promise) {
                if (promise.returnval === true) {
                    if (promise.ismintrD_Id === 0 || promise.ismintrD_Id < 0) {
                        swal('Message sent successfully');
                        $('.modal-backdrop').remove();
                        $scope.onmessageclick();
                    }
                }
                else {
                    if (promise.ismintrD_Id === 0 || promise.ismintrD_Id < 0) {
                        swal('Failed to send, please contact administrator');
                    }
                    else if (promise.ismintrD_Id > 0) {
                        swal('Failed to update, please contact administrator');
                    }
                }
                $('.modal-backdrop').remove();
            });
        };
        $scope.interactedINT = function (field) {
            return $scope.submitted1;
        };

        $scope.closemessage = function () {
            $scope.obj.groupOrIndFlg = "";
            $scope.roletype = "";
            $scope.obj.groupOrIndFlg = "";
            $scope.obj.ismintR_Subject = "";
            $scope.obj.ismintR_Interaction = "";
            $scope.obj.hrmE_Id.HRME_Id = "";
            $scope.get_toEmplist = [];
            $scope.arraymessage = "";
            $scope.obj.ismintrD_Interaction = "";
            $scope.showreply = false;
        };
        $scope.closereply = function () {
            $scope.obj.ismintrD_Interaction = "";
            $scope.showreply = false;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.SearchTaskNo = function (taskobj) {
            if ($scope.TaskNo !== undefined && $scope.TaskNo !== null && $scope.TaskNo !== "") {
                $scope.searchValueallsearch = "";
                $scope.get_globalsearchtask = [];
                $scope.currentPage = 1;
                $scope.itemsPerPage = 10;
                $scope.currentPage1 = 1;
                $scope.itemsPerPage1 = 20;
                var data = {
                    "ISMTCR_TaskNo": $scope.TaskNo
                };
                apiService.create("ISM_ComplaintsTaskList/GetTaskSearchDetails", data).then(function (promise) {
                    $scope.get_globalsearchtask = promise.get_globalsearchtask;
                    if ($scope.get_globalsearchtask !== null && $scope.get_globalsearchtask.length > 0) {
                        $('#myModalTaskList1').modal('show');
                    }
                    else {
                        $scope.TaskNo = "";
                        swal("No Records Found, Enter Existing Task Number");
                        $('#myModalTaskList1').modal('hide');
                    }
                });
            } else {
                $scope.TaskNo = "";
                swal("Enter Task Number");
            }
        };


        $scope.OnClickSearch = function () {
            $scope.searchValueall = "";
            $scope.TaskNo = "";
            $('#myModalTaskList1').modal('hide');
        };

        $scope.authDecative = function (check) {
            var dystring = "";
            if (check == false) {
                dystring = "Deactivate";
            }
            else if (check == true) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " OTP based login?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        if (ip != "" && ip != undefined && $scope.iposbrowser == "") {
                            $scope.iposbrowser = ip.ip + "," + ip.city + "," + ip.region + "," + ip.country + "," + Detect.browser + "," + Detect.version + "," + Detect.OS;
                        }
                        var data = {
                            "TwoFactorEnabled": check,
                            "logindetails": $scope.iposbrowser
                        };
                        apiService.create("Login/userauth", data).then(function (promise) {
                            if (promise.emailid != "" && promise.emailid != null) {
                                if (check == true) {
                                    if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                        $scope.$apply(function () {
                                            $scope.userauth = true;
                                        });
                                    }
                                    else {
                                        $scope.userauth = true;
                                    }

                                    swal("OTP based login " + dystring + "d Successfully!", "OTP will be shared to " + promise.emailid + " Email-Id & Mobile Notification!");
                                }
                                else {
                                    if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                        $scope.$apply(function () {
                                            $scope.userauth = false;
                                        });
                                    }
                                    else {
                                        $scope.userauth = false;
                                    }
                                    swal("OTP based login " + dystring + "d Successfully!");
                                }
                            }
                            else {
                                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                    $scope.$apply(function () {
                                        $scope.userauth = !check;
                                    });
                                }
                                else {
                                    $scope.userauth = !check;
                                }
                                swal("No Default Email-Id Exists!");
                            }
                        });
                        //$scope.$apply(function () {
                        //    $scope.userauth = check;
                        //});
                    }
                });
        };

        $scope.viewresponse1 = function (taskobj, fileview, taskindmulti, pagenomulti, f) {
            $scope.submitted1 = false;
            $scope.statusNamemodal = "";
            $scope.tasktitle = "";
            $scope.taskno = "";
            $scope.tasktype = "";
            $scope.tsakpriority = "";
            $scope.priorityswitch = "";
            $scope.prioritypreswitch = "";
            $scope.created_by = "";
            $scope.Rep_assignedto = "";
            $scope.leftflag = "";
            $scope.Rep_module = "";
            $scope.taskdate = "";
            $scope.taskstatus = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.effhrstemp = "";
            $scope.effmintemp = "";
            $scope.taskdescription = "";
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_taskdetails = [];
            $scope.objupdate.eachTaskMaxDuration = 0;
            $scope.objupdate.durationFlg = "";
            $scope.get_responseattachments = [];
            $scope.taskidmulti = taskobj.ISMTCR_Id;
            var data = {
                "ISMTCR_Id": $scope.taskidmulti
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    if (promise.role_flag == "Head") {
                        $scope.headornot = true
                    }
                    else {
                        $scope.headornot = false
                    }
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {

                        $scope.userhrmeid = promise.hrmE_Id;
                        $scope.createdhrmeid = $scope.get_taskdetails[0].createdhrmeid;
                        $scope.crdprthead = $scope.get_taskdetails[0].crdprthead;
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.Rep_module = $scope.get_taskdetails[0].IVRMM_ModuleName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.priorityswitch = $scope.get_taskdetails[0].priorityswitch;
                        $scope.prioritypreswitch = $scope.get_taskdetails[0].preswitch;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.Rep_assignedto = $scope.get_taskdetails[0].assignedto;
                        $scope.leftflag = $scope.get_taskdetails[0].empleft;

                        $scope.Responsemodal = taskobj.ismtcrreS_Response;
                        $scope.created_by = $scope.get_taskdetails[0].createdby;
                        if ($scope.get_taskdetails[0].ISMTCRASTO_StartDate != null) {
                            $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                            $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        }
                        $scope.effort = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.eachTaskMaxDuration = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.durationFlg = "HOURS";
                        var decimaltimetot = $scope.effort;
                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                        $scope.objupdate.effhrs = hrstot;
                        $scope.objupdate.effmin = mintot;
                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }
                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }
                        $scope.effhrstemp = hrstot;
                        $scope.effmintemp = mintot;
                        $scope.statusNamemodal = taskobj.ismmistS_StatusName;

                        $scope.taskAttatchment = promise.get_taskattachments;
                        $scope.get_responseattachments = promise.get_responseattachments;
                        if ($scope.taskAttatchment.length > 0) {
                            angular.forEach($scope.taskAttatchment, function (at) {
                                if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                    var img = at.ISMTCRAT_Attatchment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    at.filetype = lastelement;
                                }
                            });
                        }
                        $scope.respattachement = false;
                        if ($scope.get_responseattachments.length > 0) {
                            angular.forEach($scope.get_responseattachments, function (at) {
                                if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                    var img1 = at.ISMTCRRES_ResponseAttachment;
                                    var imagarr1 = img1.split('.');
                                    var lastelement1 = imagarr1[imagarr1.length - 1];
                                    at.filetype = lastelement1;
                                    $scope.respattachement = true;
                                }
                            });
                        }
                        if ($scope.get_taskresponse.length > 0) {
                            if ($scope.get_responseattachments.length > 0) {
                                angular.forEach($scope.get_taskresponse, function (at) {
                                    var slno = 0;
                                    angular.forEach($scope.get_responseattachments, function (atr) {
                                        if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                            slno = slno + 1;
                                            atr.slno = slno;
                                        }
                                    });
                                    //var lines = 1;
                                    //lines = at.ISMTCRRES_Response.split(/\r\n|\r|\n/).length;
                                    //at.lines = lines;
                                    at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                });
                            }
                        }
                        $('#myModalResponse1').modal('show');
                    }
                    else {
                        $('#myModalResponse1').modal('hide');
                    }
                });
        };
        $scope.closeresponse = function () {
            $('#myModalPreview').modal('show');
            $('#myModalResponse1').modal('hide');
        };
        $scope.closeview = function () {
            $('#myModalPreview').modal('hide');
            $('#showpdfg').modal('hide');
            $('#myModaldocxg').modal('hide');
            $('#myvideoPreviewg').modal('hide');
            $('#myaudioPreviewg').modal('hide');
            $('#myModalResponse1').modal('show');
        };

        //=========== View Response Image
        $scope.previewimg = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#showpdfg').modal('hide');
            $('#myModal2').modal('hide');
            $('#myModaldocxg').modal('hide');
            $('#myvideoPreviewg').modal('hide');
            $('#myaudioPreviewg').modal('hide');
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }

            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
                if (typ == 1) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRRES_ResponseAttachment;
                }


                var namedoc = "";
                if (docpath != null && docpath != undefined) {
                    $scope.docname = namedoc;
                    $scope.docnameno = $scope.docindex + 1;
                    $('#preview1').attr('src', docpath);
                    $('#myModalResponse1').modal('hide');

                    $('.modal-backdrop').remove();
                    $('#myModalPreviewimgg').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                }
            } else {
                if (typ == 1) {
                    $scope.docname = data.ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    $scope.docname = data.ISMTCRRES_ResponseAttachment;
                    angular.forEach($scope.attachmentsshow, function (pri) {
                        if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                            $scope.docname = pri.ISMTCRRES_ResponseAttachment;
                        }
                    });
                }
                $scope.docnameno = $scope.docindex + 1;
                $('#preview1').attr('src', $scope.docname);
                $('#myModalResponse1').modal('hide');

                $('.modal-backdrop').remove();
                $('#myModalPreviewimgg').modal('show');
                $('.modal').on('hide.bs.modal', function (e) {
                    e.stopPropagation();
                    $('body').css('padding-right', '');
                });
            }
        };
        $scope.downloadpdf = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse1').modal('hide');
            $('#myModalPreviewimgg').modal('hide');
            $('#myModaldocxg').modal('hide');
            $('#myvideoPreviewg').modal('hide');
            $('#myaudioPreviewg').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }

            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }

            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                imagedownload1 = docpath;
            }

            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz1");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdfg').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                });

        }
        $scope.viewdox = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse1').modal('hide');
            $('#myModalPreviewimgg').modal('hide');
            $('#showpdfg').modal('hide')
            $('#myvideoPreviewg').modal('hide');
            $('#myaudioPreviewg').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {

                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            }

            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.imagedownload1 = docpath;
            }

            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);

            $('#myModaldocxg').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        }
        $scope.viewvideo = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse1').modal('hide');
            $('#myModalPreviewimgg').modal('hide');
            $('#showpdfg').modal('hide');
            $('#myModaldocxg').modal('hide');
            $('#myaudioPreviewg').modal('hide');
            $scope.view_videos = [];
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myvideoPreviewg').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        }
        $scope.viewaudio = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse1').modal('hide');
            $('#myModalPreviewimgg').modal('hide');
            $('#showpdfg').modal('hide');
            $('#myModaldocxg').modal('hide');
            $('#myvideoPreviewg').modal('hide');
            $scope.view_videos = [];
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;

            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myaudioPreviewg').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        }

    }]);
