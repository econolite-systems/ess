// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Status.Db;
using Econolite.Ode.Status.Ess;

namespace Econolite.Ode.Repository.Ess
{
    public interface IEssStatusRepository
    {
        /// <summary>
        ///     Inserts a new environmental sensor status object into the time-series collection.
        /// </summary>
        /// <param name="statusMessage">The new environmental sensor status to insert</param>
        Task InsertAsync(EssStatus statusMessage);

        /// <summary>
        ///     Finds the most recent environmental sensor status for a given device ID or returns null if none exists.
        ///     exists.
        /// </summary>
        /// <param name="deviceId">The ID of an environmental sensor device</param>
        /// <returns>Environmental sensor status if found, otherwise null</returns>
        Task<EssStatusDocument?> FindLatest(Guid deviceId);

        /// <summary>
        ///     Finds the most recent environmental sensor status for all devices.
        /// </summary>
        /// <returns>A list of the most recent environmental sensor statuses for all devices</returns>
        Task<List<EssStatusDocument>> FindAllLatest();

        /// <summary>
        ///     Finds Environmental Sensor statuses that match a given list of device IDs with a timestamp between the
        ///     given start and end dates. If no device IDs are given, returns all statuses with a timestamp between the
        ///     start and end dates for any device. If the end date is not given, returns all statuses with a timestamp
        ///     after the given start date for the given device IDs.
        /// </summary>
        /// <param name="deviceIds">List of device IDs to filter by or empty list to filter none</param>
        /// <param name="startDate">Mandatory start date for filtering status entries</param>
        /// <param name="endDate">Optional end date for filtering status entries</param>
        /// <returns>A list of environmental sensor status objects</returns>
        Task<List<EssStatusDocument>> Find(List<Guid> deviceIds, DateTime startDate, DateTime? endDate);
    }
}
