app.service('httpInterceptor', ['$q', '$injector', "$window", "$location", function ($q, $injector, $window, $location) {

    var responseCount = 0;
    var httpInterceptor = {
        'responseError': function (response) {
            console.log("-以下是responseError");
            console.log(response);
            //to do
            angular.element("#loading").fadeOut();
            var modalService = $injector.get("ModalService");
            if (response.status === 401) {
                //这里清掉，在登陆页面就可以判断，如果有token就直接跳
                $window.localStorage.removeItem("ApiToken");
                window.location = "login.html" + "?returnUrl=" + encodeURIComponent(location.href);
            } else {
                modalService.Message(response.statusText, "响应错误");
            }
            return $q.reject(response);
        },
        'response': function (response) {

            if (response.config.url.indexOf("/api/") < 0) {
                return response || $q.when(response);
            }
            if (responseCount === 0) {
                var oldCount;
                var interval = setInterval(function () {
                    if (oldCount !== responseCount) {
                        oldCount = responseCount;
                    } else {
                        if (responseCount !== 0) {
                            angular.element("#loading").fadeOut();
                        }
                        responseCount = 0;
                        clearInterval(interval);
                    }
                }, 200);
            }
            responseCount++;

            var result = response.data;
            var modalService = $injector.get("ModalService");

            if (response.status === 200) {
                var header = result.isSuccess ? "提示" : "错误";
                if (result.isShowMsg) {
                    //angular.element("#loading").fadeOut();
                    modalService.Message(result.msg, header);
                }
                if (result.isSuccess == false) {
                    throw result;
                    //$q.when(result)
                    //$q.reject(result)
                }
            }
            return result || $q.when(result);
        },
        'request': function (config) {
            if (config.url.indexOf("/api/") < 0) {
                return config || $q.when(config);
            }

            angular.element("#loading").fadeIn();
            config.headers = config.headers || {};

            var token = $window.localStorage.ApiToken;
            if (token) {
                //config.headers.ut = $localStorage.token; 
                config.headers['X-Access-Token'] = token;
            }
            return config || $q.when(config);
        },
        'requestError': function (config) {
            console.log("-以下是requestError");
            console.log(config);

            //to do
            angular.element("#loading").fadeOut();
            //var modalService = $injector.get("ModalService");
            //modalService.Message(config.statusText, "请求错误");

            return $q.reject(config);
        }
    };
    return httpInterceptor;
}]);