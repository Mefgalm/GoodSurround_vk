angular.module('GoodSurround').controller('UsersController', ['$scope',
    function ($scope) {
        $scope.users = [{
            avatar: 'http://a3.mzstatic.com/us/r30/Purple/v4/e0/9d/64/e09d64ec-b4f1-5e55-3599-308e29d5a94d/icon100x100.png',
            name: 'Dmitry Outstanding',
            albumsCount: 34,
            songsCount: 7989
        }, {
            avatar: 'http://chandra.harvard.edu/photo/2016/gj3253/dwarfs_gj3253_xray_thm100.jpg',
            name: 'Dmitry Glorious',
            albumsCount: 4,
            songsCount: 789
        }, {
            avatar: 'http://a4.mzstatic.com/us/r30/Purple5/v4/12/a1/ef/12a1ef25-7ce6-d918-8bb9-07e5aa92fc65/icon100x100.jpeg',
            name: 'Dmitry Fabulous',
            albumsCount: 3,
            songsCount: 105
        }];
    }]);
