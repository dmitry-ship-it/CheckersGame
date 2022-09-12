export default class ApiRouter {
  private static readonly _baseUrl = "https://localhost:7167/api/game/";

  public static async get(path: string) {
    const response = await fetch(this._baseUrl + path);
    const data = await response.json();
    return data;
  }

  public static async post(path: string, body: any) {
    const response = await fetch(this._baseUrl + path, {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    });
    const data = await response.json();
    return data;
  }
}
