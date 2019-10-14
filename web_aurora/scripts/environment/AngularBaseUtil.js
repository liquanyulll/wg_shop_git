var AngularBaseUtil = angular.module('AngularBaseUtil', ['ui.bootstrap']);


AngularBaseUtil.directive('multipleEmail',
    [
        function () {
            return {
                require: "ngModel",
                link: function (scope, element, attr, ngModel) {
                    if (ngModel) {
                        var emailsRegexp = /^([a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9-_]+(\.[a-z]+)*[;；]?)+$/i;
                    }
                    var customValidator = function (value) {
                        var validity = ngModel.$isEmpty(value) || emailsRegexp.test(value);
                        ngModel.$setValidity("multipleEmail", validity);
                        return validity ? value : undefined;
                    };
                    ngModel.$formatters.push(customValidator);
                    ngModel.$parsers.push(customValidator);
                }
            };
        }
    ]);


AngularBaseUtil.filter('cutString', function () {
    return function (input, stringMaxLength) {
        if (input) {
            var maxLength = parseInt(stringMaxLength);
            if (input.length <= maxLength) {
                return input;
            } else {
                return input.substr(0, maxLength - 1) + '…';
            }
        }
    };
});

AngularBaseUtil.filter('trustHtml', function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    };
});

//百分比
AngularBaseUtil.filter('PercentValue', function () {
    return function (o, num) {
        if (o !== undefined && /(^(-)*\d+\.\d*$)|(^(-)*\d+$)/.test(o)) {
            var v = parseFloat(o);
            if (num === undefined) {
                num = 2;
            }
            return Number(Math.round(v * 10000) / 100).toFixed(num) + "%";
        } else {
            return "undefined";
        }
    };
});


AngularBaseUtil.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(false);
}]);

AngularBaseUtil.service('UrlService', ['$location', function ($location) {
    this.getUrlParam = function (keyword) {
        var reg = new RegExp("(^|&)" + keyword + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg); //匹配目标参数
        if (r !== null) return unescape(r[2]);
        return null; //返回参数值

        //return $location.search()[keyword]; 该方法在将URL开启html5Mode的时候可用，如果不开启，就不可用，但是开启的话，会造成后台MVC寻址的困扰，因此本网站默认不开启，于是找参数的方法就由Jquery代劳
    };

    this.returnHome = function () {
        return window.location = "/";
    };
}]);

AngularBaseUtil.service('RandomService', function () {
    this.randomString = function (len) {
        len = len || 32;
        var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678'; /****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/
        var maxPos = $chars.length;
        var pwd = '';
        for (i = 0; i < len; i++) {
            pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
        }
        return pwd;
    };

    this.GetRandomNum = function (Min, Max) {
        var Range = Max - Min;
        var Rand = Math.random();
        return (Min + Math.round(Rand * Range));
    };

    this.GetGuid = function () {
        var s = [];
        var hexDigits = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
            s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
        s[8] = s[13] = s[18] = s[23] = "-";

        var uuid = s.join("");
        return uuid;
    };

});

AngularBaseUtil.service('ModalService', ['$uibModal', '$log', function ($uibModal, $log) {
    this.ShowMsg = function (msg, header, afterOk) {
        var $ctrl = this;
        $ctrl.msg = msg;
        $ctrl.head = (header === undefined || header === '' || header === null) ? "提示" : header;

        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: '/drstc/tl/ViewTemplate/ErrorModal.html',
            windowClass: 'uibModalWindow',
            controller: function ($scope) {
                $scope.msg = $ctrl.msg;
                $scope.head = $ctrl.head;
                $scope.Ok = function () {
                    modalInstance.dismiss();
                    if (afterOk !== undefined && afterOk !== null)
                        afterOk();
                };
            },
            size: 'mg'
        });

        return;
    };

    this.ShowRepeatMsg = function (repeatMsg, header) {
        var $ctrl = this;
        $ctrl.msg = repeatMsg;
        $ctrl.head = (header === undefined || header === '' || header === null) ? "提示" : header;

        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: '/drstc/tl/ViewTemplate/RepeatMsgModal.html',
            windowClass: 'uibModalWindow',
            controller: function ($scope) {
                $scope.msg = $ctrl.msg;
                $scope.head = $ctrl.head;
                $scope.Ok = function () {
                    modalInstance.dismiss();
                };
            },
            size: 'mg'
        });

        return;
    };

    this.ShowConfirm = function (msg, afterOk) {
        var $ctrl = this;
        $ctrl.msg = msg;
        $ctrl.head = "请确认";

        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: '/drstc/tl/ViewTemplate/ConfirmModal.html',
            windowClass: 'uibModalWindow',
            controller: function ($scope) {
                $scope.msg = $ctrl.msg;
                $scope.head = $ctrl.head;
                $scope.Ok = function () {
                    modalInstance.close();
                };
                $scope.Cancel = function () {
                    modalInstance.dismiss('cancel');
                };
            },
            size: 'mg'
        });

        modalInstance.result.then(function (selectedItem) {
            //当选择OK之后做啥
            afterOk();
        }, function () {
            //当选择关闭Modal后做啥
        });

        return;
    };


    this.ShowErrMsg = function (data, httpStatus) {
        switch (httpStatus) {
            case 404:
                this.ShowMsg("请求的资源不存在。", "错误");
                break;
            case 0:
                this.ShowMsg("服务器超时。", "错误");
                break;
            case 401:
                this.ShowMsg("会话超时，请重新登录。", "错误");
                break;
            case 403:
                window.location = "/home/login";
                break;
            case 501:
                this.ShowMsg(data, "提示");
                break;
            case 500:
                this.ShowMsg(data, "错误");
                break;
            case 599: //脉保自定义Http错误
                this.ShowMsg(data);
                break;
            default:
                var msg = "";
                if (data !== null) {
                    if (typeof (data.ExceptionMessage) !== "undefined") {
                        msg += data.ExceptionMessage + "\n";
                    } else if (typeof (data.Message) !== "undefined") {
                        msg += data.Message + "\n";
                    } else if (typeof (data.MessageDetail) !== "undefined") {
                        msg += data.MessageDetail + "\n";
                    } else {
                        msg = data;
                    }
                }
                if (httpStatus !== null) {
                    this.ShowMsg(httpStatus + ":" + msg, "错误");
                } else {
                    this.ShowMsg(msg, "错误");
                }
                break;
        }

        return;
    };


}]);


AngularBaseUtil.service('HttpCatchService', ['ModalService', function (modalService) {
    this.CatchError = function (resp) {
        modalService.ShowErrMsg(resp.data, resp.status);
        return;
    };
}]);

AngularBaseUtil.directive('loadingModal', ['$http', function ($http) {
    return {
        restrict: 'EA',
        templateUrl: '/drstc/tl/viewtemplate/loadingmodal.html',
        scope: true,
        link: function (scope, elm, attrs) {
            scope.isloadingFun = function () { return $http.pendingRequests.length > 0; }
            scope.$watch(scope.isloadingFun,
                function (v) {
                    if (v) {
                        scope.isLoading = true;
                    } else {
                        scope.isLoading = false;
                    }
                });
        }
    };
}]);

AngularBaseUtil.directive('controlledLoading', ['$http', function ($http) {
    return {
        restrict: 'EA',
        templateUrl: '/drstc/tl/viewtemplate/ControlledLoading.html',
        scope: false,
        link: function (scope, elm, attrs) {
            scope.$watch(scope.showLoading,
                function (v) {
                    if (v) {
                        $('#controlledLoading').show();
                    } else {
                        $('#controlledLoading').hide();
                    }
                });
        }
    };
}]);


AngularBaseUtil.directive('datePicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.datepicker({
                    dateFormat: 'yy-mm-dd',
                    defaultDate: '-25y',
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (date) {
                        scope.$apply(function () {
                            ngModelCtrl.$setViewValue(date);
                        });
                    }
                });
            });
        }
    };
});


AngularBaseUtil.factory('anchorScroll', function () {
    function toView(element, top, height) {
        var winHeight = $(window).height();

        element = $(element);
        height = height > 0 ? height : winHeight / 10;
        $('html, body').animate({
            scrollTop: top ? (element.offset().top - height) : (element.offset().top + element.outerHeight() + height - winHeight)
        }, {
                duration: 200,
                easing: 'linear',
                complete: function () {
                    if (!inView(element)) {
                        element[0].scrollIntoView(!!top);
                    }
                }
            });
    }

    function inView(element) {
        element = $(element);

        var win = $(window),
            winHeight = win.height(),
            eleTop = element.offset().top,
            eleHeight = element.outerHeight(),
            viewTop = win.scrollTop(),
            viewBottom = viewTop + winHeight;

        function isInView(middle) {
            return middle > viewTop && middle < viewBottom;
        }

        if (isInView(eleTop + (eleHeight > winHeight ? winHeight : eleHeight) / 2)) {
            return true;
        } else if (eleHeight > winHeight) {
            return isInView(eleTop + eleHeight - winHeight / 2);
        } else {
            return false;
        }
    }

    return {
        toView: toView,
        inView: inView
    };
});

AngularBaseUtil.service('HttpCatchService', ['ModalService', function (ModalService) {
    this.CatchError = function (resp) {
        if (resp.data === undefined) {
            ModalService.ShowMsg("前台错误，请看控制台");
            console.error(resp);
        } else {
            ModalService.ShowMsg(resp.data.Message === undefined ? resp.data : resp.data.Message, "错误");
        }

        return;
    };
}]);

AngularBaseUtil.service('MBScroll', ['$timeout', '$document', function ($timeout, $document) {
    this.scrollTo = function (partId, scrollToCallBack) {
        $timeout(function () {
            var duration = 500; //milliseconds
            var offset = 10; //pixels; adjust for floating menu, context etc
            var someElement = angular.element(document.getElementById(partId));
            if (someElement !== null) {
                $document.scrollToElement(someElement, offset, duration).then(function () {
                    if (scrollToCallBack !== undefined && typeof (scrollToCallBack) === "function") {
                        scrollToCallBack();
                    }
                });
            }
        },
            100);
    };
}]);

AngularBaseUtil.service('NgFormService', [function () {
    this.ResetForm = function (currentScope, form) {
        form.$setPristine();
        currentScope.$broadcast("NgFormRest", form); //发送NgFormRest，告知所有需要监听该事件的指令，配合进行FormReset
    };
}]);


AngularBaseUtil.directive('errorBorder', [function () {
    return {
        restrict: 'EA',
        require: 'ngModel',
        link: function (scope, elm, attrs, c) {
            elm.blur(function (event) {
                if (c.$dirty && c.$invalid) {
                    elm.css("border", "3px solid red");
                } else {
                    elm.css("border", "");
                }
            });

            //监听NgFormRest广播，如果前台想要RestForm，那么这边需要把已经置为红色边框的输入项也恢复到最初的样子
            scope.$on("NgFormRest",
                function (e, form) {
                    if (c.$$parentForm === form) {
                        elm.css("border", "");
                    }
                });

        }
    };
}]);

