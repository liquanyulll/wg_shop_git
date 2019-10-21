app.config(['$stateProvider', "$urlRouterProvider", "$httpProvider", function ($stateProvider, $urlRouterProvider, $httpProvider) {
    //这是因为Angular 1.6 版本更新后 对路由做的处理，这样才可以和以前版本一样正常使用
    //$locationProvider.hashPrefix('');
    //注册拦截器
    $httpProvider.interceptors.push("httpInterceptor");

    $urlRouterProvider.when("", "/index");
    $stateProvider
        .state("index", {
            url: "/index",
            views: {
                '': {
                    templateUrl: 'index.html'
                },
                'head@index': {
                    templateUrl: 'views/main/head.html',
                    controller: "headCtrl"
                },
                'navbar@index': {
                    templateUrl: 'views/main/navbar.html',
                    controller: "navbarCtrl"
                }
            }
        });
        //.state("index.Page1", {
        //    params: { ddf: null, product_id: null, accp: null },
        //    //url: '/{contactId:[0-9]{1,4}}',
        //    //url: "/Page1/:product_id/:accp",
        //    url: "/Page1/:product_id/:accp",
        //    templateUrl: "Page1.html",
        //    controller: "page1"
        //})
        //.state("PageTab.Page2", {
        //    url: "/Page2",
        //    templateUrl: "Page2.html"
        //})
        //.state("PageTab.Page3", {
        //    url: "/Page3",
        //    templateUrl: "Page3.html",
        //    controller: "page3"
        //})
        //.state("PageTab.Page3.Page4", {
        //    url: "/Page4",
        //    templateUrl: "Page4.html"
        //});

    //$routeProvider
    //    .when('/productList', {
    //        /**/
    //        templateUrl: 'views/product-list.html',
    //        /**/
    //        controller: "ProductListCtrl"
    //    }).when('/product/:id/:invcode?', {
    //        /**/
    //        templateUrl: 'views/product.html',
    //        /**/
    //        controller: "ProductCtrl"
    //    }).when('/my', {
    //        /**/
    //        templateUrl: 'views/my.html',
    //        /**/
    //        controller: "MyCtrl"
    //    })//).when('/webSite', {
    //    //    /**/
    //    //    templateUrl: 'views/web-site.html',
    //    //    /**/
    //    //    controller: "WebSiteCtrl"
    //    //}).when('/preProcess', {
    //    //    /**/
    //    //    templateUrl: 'views/pre-process.html',
    //    //    /**/
    //    //    controller: "PreProcessCtrl"
    //    //}).when('/publishInfo', {
    //    //    /**/
    //    //    templateUrl: 'views/publish-info.html',
    //    //    /**/
    //    //    controller: "PublishInfoCtrl"
    //    //}).when('/login', {
    //    //    /**/
    //    //    templateUrl: 'views/login.html',
    //    //    /**/
    //    //    controller: "LoginCtrl"
    //    //})
    //    .otherwise({
    //        redirectTo: 'productList'
    //    });

}]);