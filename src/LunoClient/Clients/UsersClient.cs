﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Luno.Abstracts;
using Luno.Connections;
using Luno.Interfaces;
using Luno.Models;
using Luno.Models.Event;
using Luno.Models.User;

namespace Luno.Clients
{
	public class UsersClient
		: ApiClient, IUsersClient
	{
		public UsersClient(ApiKeyConnection connection)
			: base(connection)
		{ }
		
		public async Task<User<T>> CreateAsync<T>(CreateUser<T> user, bool autoName = true, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			additionalParams.Add("auto_name", autoName.ToString().ToLowerInvariant());
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.PostAsync<User<T>>("/users", user, additionalParams);
		}
		
		#region [ GetAsync ]

		public async Task<User<T>> GetAsync<T>(string id, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.GetAsync<User<T>>($"/users/{id}", additionalParams);
		}

		public async Task<User<T>> GetAsync<T>(User<T> user, string[] expand = null)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await GetAsync<T>(user.Id, expand: expand);
		}

		#endregion

		public async Task<PaginationResponse<User<T>>> GetAllAsync<T>(string from = null, string to = null, uint limit = 100, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			additionalParams.Add(nameof(limit), limit.ToString());
			if (from != null) additionalParams.Add(nameof(from), from);
			if (to != null) additionalParams.Add(nameof(to), to);
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.GetAsync<PaginationResponse<User<T>>>("/users", additionalParams);
		}

		#region [ UpdateAsync ]

		public async Task<SuccessResponse> UpdateAsync<T>(string id, User<T> user, bool autoName = true, bool destructive = false)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			var additionalParams = new Dictionary<string, string>();
			additionalParams.Add("auto_name", autoName.ToString().ToLowerInvariant());

			var updateUser = new UpdateUser<T>
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Name = user.Name,
				Profile = user.Profile,
				Username = user.Username
			};

			if (destructive)
				return await HttpConnection.PutAsync<SuccessResponse>($"/users/{id}", updateUser, additionalParams);
			else
				return await HttpConnection.PatchAsync<SuccessResponse>($"/users/{id}", updateUser, additionalParams);
		}

		public async Task<SuccessResponse> UpdateAsync<T>(User<T> updatedUser, bool autoName = true, bool destructive = false)
		{
			Ensure.ArgumentNotNull(updatedUser, nameof(updatedUser));

			return await UpdateAsync(updatedUser.Id, updatedUser, autoName: autoName, destructive: destructive);
		}

		#endregion

		#region [ DeactivateAsync ]

		public async Task<SuccessResponse> DeactivateAsync(string id)
		{
			return await HttpConnection.DeleteAsync<SuccessResponse>($"/users/{id}");
		}

		public async Task<SuccessResponse> DeactivateAsync<T>(User<T> user)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await DeactivateAsync(user.Id);
		}

		#endregion

		#region [ ValidatePasswordAsync ]

		public async Task<SuccessResponse> ValidatePasswordAsync(string id, string password)
		{
			return await HttpConnection.PostAsync<SuccessResponse>($"/users/{id}/password/validate", new { password });
		}

		public async Task<SuccessResponse> ValidatePasswordAsync<T>(User<T> user, string password)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await ValidatePasswordAsync(user.Id, password);
		}

		#endregion

		#region [ ChangePasswordAsync ]

		public async Task<SuccessResponse> ChangePasswordAsync(string id, string newPassword, string currentPassword = null)
		{
			return await HttpConnection.PostAsync<SuccessResponse>($"/users/{id}/password/change", 
				new { password = newPassword, current_password = currentPassword });
		}

		public async Task<SuccessResponse> ChangePasswordAsync<T>(User<T> user, string newPassword, string currentPassword = null)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await ChangePasswordAsync(user.Id, newPassword, currentPassword: currentPassword);
		}

		#endregion

		#region [ CreateEventAsync ]

		public async Task<Event<TEvent, TUser>> CreateEventAsync<TEvent, TUser>(string id, CreateEvent<TEvent> @event, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.PostAsync<Event<TEvent, TUser>>(
				$"/users/{id}/events", @event, additionalParams);
		}

		public async Task<Event<TEvent, TUser>> CreateEventAsync<TEvent, TUser>(User<TUser> user, CreateEvent<TEvent> @event, string[] expand = null)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await CreateEventAsync<TEvent, TUser>(user.Id, @event, expand: expand);
		}

		#endregion

		#region [ GetEventsAsync ]

		public async Task<PaginationResponse<Event<TEvent, TUser>>> GetEventsAsync<TEvent, TUser>(string id, string from = null, string to = null, uint limit = 100, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			additionalParams.Add(nameof(limit), limit.ToString());
			if (from != null) additionalParams.Add(nameof(from), from);
			if (to != null) additionalParams.Add(nameof(to), to);
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.GetAsync<PaginationResponse<Event<TEvent, TUser>>>($"/users/{id}/events", additionalParams);
		}

		public async Task<PaginationResponse<Event<TEvent, TUser>>> GetEventsAsync<TEvent, TUser>(User<TUser> user, string from = null, string to = null, uint limit = 100, string[] expand = null)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await GetEventsAsync<TEvent, TUser>(user.Id, from: from, to: to, limit: limit, expand: expand);
		}

		#endregion
		
		public async Task<LoginResponse<TUser, TSession>> LoginAsync<TUser, TSession>(string login, string password, string[] expand = null)
		{
			var additionalParams = new Dictionary<string, string>();
			if (expand != null) additionalParams.Add(nameof(expand), string.Join(",", expand));

			return await HttpConnection.PostAsync<LoginResponse<TUser, TSession>>("/users/login", new { login, password }, additionalParams);
		}
		
		#region [ DeleteSessionsAsync ]

		public async Task<SuccessResponse> DeleteSessionsAsync(string id)
		{
			return await HttpConnection.DeleteAsync<SuccessResponse>($"/users/{id}/sessions");
		}

		public async Task<SuccessResponse> DeleteSessionsAsync<T>(User<T> user)
		{
			Ensure.ArgumentNotNull(user, nameof(user));

			return await DeleteSessionsAsync(user.Id);
		}

		#endregion
	}
}
