angular.module('GoodSurround', ['ngRoute']);

angular.module('GoodSurround').config(['$routeProvider', '$httpProvider', '$locationProvider',
function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider.
    when('/', {
        templateUrl: 'templates/home.html',
        controller: 'HomeController'
    }).
    when('/find', {
        templateUrl: 'templates/find.html',
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
