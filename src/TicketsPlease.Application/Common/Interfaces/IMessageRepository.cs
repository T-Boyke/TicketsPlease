// <copyright file="IMessageRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht für <see cref="Message"/> Entitäten.
/// </summary>
public interface IMessageRepository
{
  /// <summary>
  /// Ruft eine Nachricht anhand ihrer ID ab.
  /// </summary>
  /// <param name="id">Die ID der Nachricht.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die Nachricht oder null.</returns>
  public Task<Message?> GetByIdAsync(Guid id, CancellationToken ct = default);

  /// <summary>
  /// Ruft alle Nachrichten für einen Benutzer ab (gesendet oder empfangen).
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Nachrichten.</returns>
  public Task<List<Message>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default);

  /// <summary>
  /// Fügt eine neue Nachricht hinzu.
  /// </summary>
  /// <param name="message">Die Nachricht.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task AddAsync(Message message, CancellationToken ct = default);

  /// <summary>
  /// Speichert Änderungen.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task SaveChangesAsync(CancellationToken ct = default);
}
