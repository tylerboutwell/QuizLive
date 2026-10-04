import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { API_URL } from "./api.js";

export function createConnection() {
    return new HubConnectionBuilder()
        .withUrl(`${API_URL}/gameHub`)
        .withAutomaticReconnect()
        .configureLogging(LogLevel.None)
        .build();
}