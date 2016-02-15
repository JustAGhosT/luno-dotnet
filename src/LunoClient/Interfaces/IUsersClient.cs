﻿using System.Threading.Tasks;
using Luno.Models;
using Luno.Models.ApiAuthentication;
using Luno.Models.Event;
using Luno.Models.Session;
using Luno.Models.User;

namespace Luno.Interfaces
{
	public interface IUsersClient
	{
		/// <summary>
		/// Create a user
		/// </summary>
		/// <typeparam name="T">Any arbitrary data associated with the user model</typeparam>
		/// <param name="user">The new user's details</param>
		/// <param name="autoName">Whether to automatically set Name, FirstName and LastName based on the provided data</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<User<T>> CreateAsync<T>(CreateUser<T> user, bool autoName = true, string[] expand = null);

		/// <summary>
		/// Get a user
		/// </summary>
		/// <typeparam name="T">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<User<T>> GetAsync<T>(string id, string[] expand = null);

		/// <summary>
		/// Get a list of recently created users
		/// </summary>
		/// <typeparam name="T">Any arbitrary data associated with the user model</typeparam>
		/// <param name="from">The item ID to retrieve the list from</param>
		/// <param name="to">The item ID to stop retrieving the list at</param>
		/// <param name="limit">The maximum number of items to return. min: 0, max: 200</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<PaginationResponse<User<T>>> GetAllAsync<T>(string from = null, string to = null, uint limit = 100, string[] expand = null);

		/// <summary>
		/// Update a user
		/// </summary>
		/// <typeparam name="T">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="updatedUser">The updated user model</param>
		/// <param name="autoName">Whether to automatically set Name, FirstName and LastName based on the provided data</param>
		/// <param name="destructive">Whether to update existing attributes, or override the model and replace it in it's entirety</param>
		Task<SuccessResponse> UpdateAsync<T>(string id, User<T> updatedUser, bool autoName = true, bool destructive = false);

		/// <summary>
		/// Deactivate a user, setting the `closed` attribute.
		/// </summary>
		/// <param name="id">The user ID</param>
		Task<SuccessResponse> DeactivateAsync(string id);

		/// <summary>
		/// Check that a password is correct, without logging the user in
		/// </summary>
		/// <param name="id">The user ID</param>
		/// <param name="password">The user's password </param>
		Task<SuccessResponse> ValidatePassword(string id, string password);

		/// <summary>
		/// Change or set a users password
		/// </summary>
		/// <param name="id">The user ID</param>
		/// <param name="newPassword">The new password </param>
		Task<SuccessResponse> ChangePassword(string id, string newPassword);

		/// <summary>
		/// Trigger an event for this user
		/// </summary>
		/// <typeparam name="TEvent">Any arbitrary data associated with the event model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="event"></param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<Event<TEvent, TUser>> CreateEventAsync<TEvent, TUser>(string id, CreateEvent<TEvent> @event, string[] expand = null);

		/// <summary>
		/// Get a list of recently triggered events by this user
		/// </summary>
		/// <typeparam name="TEvent">Any arbitrary data associated with the event model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="from">The item ID to retrieve the list from</param>
		/// <param name="to">The item ID to stop retrieving the list at</param>
		/// <param name="limit">The maximum number of items to return. min: 0, max: 200</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<PaginationResponse<Event<TEvent, TUser>>> GetEventsAsync<TEvent, TUser>(string id, string from = null, string to = null, uint limit = 100, string[] expand = null);

		/// <summary>
		/// Log in a user by id, email or username
		/// </summary>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <typeparam name="TSession">Any arbitrary data associated with the session model</typeparam>
		/// <param name="login">The user ID, email or username</param>
		/// <param name="password">The user's password </param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<LoginResponse<TUser, TSession>> LoginAsync<TUser, TSession>(string login, string password, string[] expand = null);

		/// <summary>
		/// Create a new session for this user
		/// </summary>
		/// <typeparam name="TSession">Any arbitrary data associated with the session model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="session">The new sessions's details</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<Session<TSession, TUser>> CreateSessionAsync<TSession, TUser>(string id, CreateSession<TSession> session = null, string[] expand = null);

		/// <summary>
		/// Get a list of sessions owned by this user
		/// </summary>
		/// <typeparam name="TSession">Any arbitrary data associated with the session model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="from">The item ID to retrieve the list from</param>
		/// <param name="to">The item ID to stop retrieving the list at</param>
		/// <param name="limit">The maximum number of items to return. min: 0, max: 200</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<PaginationResponse<Session<TSession, TUser>>> GetSessionsAsync<TSession, TUser>(string id, string from = null, uint limit = 100, string to = null, string[] expand = null);

		/// <summary>
		/// Permanently delete all sessions associated with this user
		/// </summary>
		/// <param name="id">The user ID</param>
		Task<SuccessResponse> DeleteSessionsAsync(string id);

		/// <summary>
		/// Create a new API key for this user
		/// </summary>
		/// <typeparam name="TApiAuthentication">Any arbitrary data associated with the api authentication model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="apiAuthentication">The new api authentication's details</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<ApiAuthentication<TApiAuthentication, TUser>> CreateApiAuthenticationAsync<TApiAuthentication, TUser>(string id, CreateApiAuthentication<TApiAuthentication> apiAuthentication = null, string[] expand = null);

		/// <summary>
		/// Get a list of API keys owned by this user
		/// </summary>
		/// <typeparam name="TApiAuthentication">Any arbitrary data associated with the api authentication model</typeparam>
		/// <typeparam name="TUser">Any arbitrary data associated with the user model</typeparam>
		/// <param name="id">The user ID</param>
		/// <param name="expand">The models to expand (fetch details)</param>
		Task<PaginationResponse<ApiAuthentication<TApiAuthentication, TUser>>> GetAllApiAuthenticationsAsync<TApiAuthentication, TUser>(string id, string[] expand = null);
	}
}
