using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Core
{

    public class InMemoryUserTracker<THub> : IUserTracker<THub>
    {
        private readonly ConcurrentDictionary<HubConnectionContext, UserDetailsDto> _usersOnline
            = new ConcurrentDictionary<HubConnectionContext, UserDetailsDto>();

        public event Action<UserDetailsDto[]> UsersJoined;
        public event Action<UserDetailsDto[]> UsersLeft;

        public Task<IEnumerable<UserDetailsDto>> UsersOnline() => Task.FromResult(_usersOnline.Values.AsEnumerable());

        public Task<IEnumerable<UserDetailsDto>> UsersOnline(string groupId)
        {
            return Task.FromResult(_usersOnline.Values.Where(u => u.GroupId == groupId).AsEnumerable());
        }

        public Task<UserDetailsDto> GetUser(HubConnectionContext connection)
        {
            var user = _usersOnline.Values.FirstOrDefault( u => u.ConnectionId == connection.ConnectionId);

            return Task.FromResult(user);
        }

        public Task AddUser(HubConnectionContext connection, UserDetailsDto userDetails)
        {
            _usersOnline.TryAdd(connection, userDetails);
            UsersJoined(new[] { userDetails });

            return Task.CompletedTask;
        }

        public Task UpdateUser(HubConnectionContext connection, UserDetailsDto userDetails)
        {
            _usersOnline.AddOrUpdate(connection, userDetails, (oldKey, oldValue) => userDetails);

            return Task.CompletedTask;
        }

        public Task RemoveUser(HubConnectionContext connection)
        {
            if (_usersOnline.TryRemove(connection, out var userDetails))
            {
                UsersLeft(new[] { userDetails });
            }

            return Task.CompletedTask;
        }
    }
}