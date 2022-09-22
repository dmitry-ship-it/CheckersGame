export default class ApiRouter {
  private static readonly _baseUrl = "https://localhost:7167/api/game/";

  public static async get(path: string) {
    const response = await fetch(this._baseUrl + path);
    const data = await response.json();
    return data;
  }

  public static async post<T>(path: string, body: T) {
    const response = await this.fetchPost<T>(path, body);

    if (response.status === 400) throw new Error("You can't do this.");

    const data = await response.json();
    return data;
  }

  public static createLongPollingPost<T>(path: string, body: T, setterCallback: any) {
    const postRequest = async (content: T) => {
      console.log("Fetching games... Now games is " + JSON.stringify(content));
      console.log("sending body: " + JSON.stringify(content));
      const response = await this.fetchPost(path, content);

      if (response.status !== 200) {
        console.warn("Fetch pending games list error");
        await postRequest(content);
      } else {
        const data = await response.json();
        setterCallback(data);
        content = data;
      }

      await postRequest(content);
    };

    postRequest(body);
  }

  private static async fetchPost<T>(path: string, body: T) {
    return fetch(this._baseUrl + path, {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    });
  }
}
