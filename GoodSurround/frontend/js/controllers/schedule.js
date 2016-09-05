angular.module('GoodSurround').controller('ScheduleController', ['$scope', '$uibModal',
    function ($scope, $uibModal) {
        $scope.displayUsers = false;

        $scope.toggleUsersPanel = function () {
            $scope.displayUsers = !$scope.displayUsers;
        };

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
        $scope.users.push({
            avatar: 'https://yt3.ggpht.com/-5mSnQcMR1E8/AAAAAAAAAAI/AAAAAAAAAAA/cW36ovOzVZw/s100-c-k-no-mo-rj-c0xffffff/photo.jpg',
            name: 'Here comes some incredibly long name which purpose is only to test it\'s displaying',
            albumsCount: 7,
            songsCount: 783
        });

        $scope.openSearchModal = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'search-people.html',
                controller: 'PeopleSearchController'
            });

            modalInstance.result.then(function success (message) {
                console.log(message);
            }, function failure () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
    }]);
