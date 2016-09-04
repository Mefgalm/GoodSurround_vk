angular.module('GoodSurround', []);

angular.module('GoodSurround').config(['$routeProvider', '$httpProvider', '$locationProvider',
function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider.
    when('/', {
        templateUrl: 'views/home.html',
        controller: 'HomeController'
    }).
    when('/find', {
        templateUrl: 'views/find.html',
        controller: 'FindController'
    }).
    otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);
