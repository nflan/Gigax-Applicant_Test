# Gigax-Applicant_Test
Create, in 3 days, a python parser and backend, get (used http request) information in Unity for NPC behavior management.

**Resources**:
- Scene assets
- Tutorials
- Advices

**Authorized help**:
- All internet ressources
- ChatGPT
    - I'm not big fan of it in code but with the given time, it helped me a lot on python part. Really less on Unity!

Objectives
--
- Learn how to create a backend in Python and make a Json parser
- Learn Unity fundamentals
    - Get request form a FastAPI Python server.
    - Manage NPC actions.
    - Add Animation to NPC.

Constraints
--
- 3 days to give the best.
- I had never coded in Python before
- I had never created a project in Unity

Feedbacks
--
The project is unfinished.
- Animations are not all set.
- Press Button Animation is triggered when moving after using it.
- Couldn't spend time on bonuses.
- Code is not as readable as it could.
- Missing a tons of error handling.

But I gave my best to have a Proof of Concept in only 3 days without any knowledge before starting. So I'm glad to:
- have managed the Python part and the requests from Unity the first day.
- have something that works (at least with the given Json).
- have shown my motivation by working 35 hours in 3 days.

I met some troubles with:
- NPC collision detection:
    - I tried to add a raycasting solution to avoid collision between NPCs but it was not a success.
- Animations:
    - I've used movements from a Unity Tutorial so when I tried to add the press button animation, it was hard to handle.
- Speechbubble:
    - what an user-friendly thing.
    - they only works in front of the NPC.
    - I took time to understand and handle it.
- Tags or names:
    - I am not sure if I managed the places and objects in the best way. I could probably tag them by position:
        - for example: all items and floor in Kitchen could have been tagged "Kitchen".
        
        I used tag to set if whether it is a 'resource' (object) or 'place' (location) so it was harder to find the 'ressource' in a given 'place'.
- Assets given:
    - Assets names don't correspond to the ones in Json file. So I had to rename 'places' and to find in the names of 'resources' if it contains the object I was looking for. Even like that, some 'resources' were not well-named, such as: "RollingTV" (json) and "Rolling_TV" (Unity). So the project is less scalable as it should be.

I have left some comments in the code to understand functions and give way of improvement.

How to use
--
- Clone the repo
- Import UnityProject part to Unity 2022.3.
- Start the server by using: `uvicorn backend:app --reload`
- Launch simulation on Unity
- Execute the parse.py file: `python3 parse.py`


