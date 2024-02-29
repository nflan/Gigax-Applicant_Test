from fastapi import FastAPI, Response
from starlette.responses import StreamingResponse
from typing import List, Optional
from pydantic import BaseModel
import asyncio

app = FastAPI()

# Ways of improvement:
#   - Security checks
#   - manage error cases

# Define a Pydantic model for the data to be sent to clients
class NPCAction(BaseModel):
    npc_name: str
    action: str
    location_name: Optional[str] = None
    object_name: Optional[str] = None
    content: Optional[str] = None
    target_name: Optional[str] = None

data_queue = asyncio.Queue()
time = 10

# I've decided to clear the queue if no get request since 10 seconds after a first get. It was better to test the project.
async def clear_queue():
    global time
    while time != 0:
        await asyncio.sleep(1)
        time -= 1
    try:
        data_queue._queue.clear()  # Clear the queue
        print("Queue cleared.")
    except Exception as e:
        print(f"Error occurred while clearing queue: {e}")

# Endpoint to handle client requests for data
@app.get("/get_data")
async def get_data():
    global time
    if time == 10:
        asyncio.create_task(clear_queue())
    time = 10
    if not data_queue.empty():
        data = await data_queue.get()
        return data
    else:
        return {"message": "No data available"}

# Endpoint to add data to the queue
@app.post("/post_data")
async def translate_action(action: NPCAction):
    await data_queue.put(action)
    return {"message": "Data added successfully"}

# Server-side event (SSE) endpoint for real-time updates
@app.get("/updates")
async def updates(response: Response):
    async def event_generator():
        while True:
            if not data_queue.empty():
                data = await data_queue.get()
                yield f"data: {data}\n\n"
            else:
                await asyncio.sleep(1)

    return StreamingResponse(event_generator(), media_type="text/event-stream")