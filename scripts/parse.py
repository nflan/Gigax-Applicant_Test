import json
import requests
from typing import List, Optional
from pydantic import BaseModel

# Ways of improvement:
#   - manage different file name and not forced the route.
#   - add error handler on the json. atm, if it not a json or if it badly configured, unexpected behavior happens

url = 'http://localhost:8000/post_data'

headers = {'Content-Type': 'application/json'}

file = './actions/NPC_actions.json'

class NPCAction(BaseModel):
    npc_name: str
    action: str
    location_name: Optional[str] = None
    object_name: Optional[str] = None
    content: Optional[str] = None
    target_name: Optional[str] = None

class NPCActions(BaseModel):
    npc_actions: List[NPCAction]

def parseJsonFile(filePath: str) -> NPCActions:
    with open(filePath, 'r') as json_file:
        data = json.load(json_file)
        npcActionsData = data.get('npc_actions', [])
        npc_actions = [NPCAction(**action) for action in npcActionsData]
        return NPCActions(npc_actions=npc_actions)

parsedData = parseJsonFile(file)
for action_list in parsedData.npc_actions:
    action_dict = action_list.dict()
    json_data = json.dumps(action_dict)
    response = requests.post(url, data=json_data, headers=headers)
    if response.status_code == 200:
        print("POST request successful")
        # You can print the response content if needed
        print(response.text)
    else:
        print("POST request failed")
        print("Status code:", response.status_code)
    print(action_list)
    print('\n')