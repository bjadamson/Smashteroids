﻿using System.Collections.Generic;
using EntityLibrary.Components.Interface;
using EntityLibrary.Entity;

namespace EntityLibrary.Repositories.EntityRepository
{
	// NOTE: this may need to be public, design not far enough
	internal interface IEntityRepository
	{
		/// <summary>
		/// Inserts an entity into the repository.
		/// </summary>
		/// <param name="entity">The entity to insert.</param>
		void InsertEntity(IEntity entity);


		/// <summary>
		/// Inserts a collection of entities into the repository.
		/// </summary>
		/// <param name="entityBatch">The entity batch.</param>
		void InsertBatchEntities(IEnumerable<IEntity> entityBatch);


		/// <summary>
		/// Removes an entity from the repository.
		/// <remarks> Sets entity to null.</remarks>
		/// </summary>
		/// <param name="entity">The entity to remove, null after completion.</param>
		void RemoveEntity(IEntity entity);


		/// <summary>
		/// Gets all entities with component.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="component">The component.</param>
		/// <returns></returns>
		IEnumerable<IEntity> GetEntitiesWithComponent<T>() where T : IComponent;


		/// <summary>
		/// Get references to each component of type T, within the entities.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IEnumerable<T> GetComponentsOfType<T>() where T : IComponent;
	}
}
