angular.module('GoodSurround', ['ngRoute']);

angular.module('GoodSurround').config(['$routeProvider', '$httpProvider', '$locationProvider',
function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider.
    when('/', {
        templateUrl: 'templates/home.html',
        controller: 'HomeController'
    }).
    when('/users', {
        templateUrl: 'templates/users.html',
        controller: 'UsersController'
    }).
    when('/schedule', {
        templateUrl: 'templates/schedule.html',
        controller: 'ScheduleController'
    }).
    otherwise({
        redirectTo: '/'
    });

    // $locationProvider.html5Mode({
    //     enabled: true,
    //     requireBase: false
    // });
}]);
