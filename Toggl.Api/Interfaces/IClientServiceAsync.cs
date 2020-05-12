﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Toggl.Api.DataObjects;

namespace Toggl.Api.Interfaces
{
	public interface IClientServiceAsync
	{
		IApiServiceAsync TogglSrv { get; set; }

		/// <summary>
		///
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/clients.md#get-clients-visible-to-user
		/// </summary>
		/// <returns></returns>
		Task<List<Client>> List(bool includeDeleted = false);

		/// <summary>
		/// Get a client
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/clients.md#get-client-projects
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Client> Get(int id);

		/// <summary>
		/// Add a client
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/clients.md#create-a-client
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		Task<Client> Add(Client obj);

		/// <summary>
		/// Edit a client
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/clients.md#update-a-client
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		Task<Client> Edit(Client obj);

		/// <summary>
		/// Delete a client
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/clients.md#delete-a-client
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> Delete(int id);
	}
}