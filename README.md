# IBM AR & AI Enhanced Next Gen Documentation

## Overview

This project is an Augmented Reality (AR) application built with Unity, designed to place and interact with 3D models in a real-world environment using image tracking. It leverages various technologies, including Unity, Blender, WatsonX, and AWS, to provide a seamless AR experience on both Android and iOS platforms. The application allows users to place 3D models over recognized images, interact with these models, and view detailed information about different components.

## Technologies Used

### Unity
Unity is the primary development platform for this AR application, providing a robust framework for building cross-platform AR experiences.

### ARFoundation and ARCore
- **ARFoundation:** A Unity package that allows developers to build AR applications across multiple platforms.
- **ARCore:** Google's platform for building AR experiences on Android devices, integrated with Unity through ARFoundation.

### Blender
Blender is used for creating and exporting 3D models that are used within the Unity project.

### WatsonX
WatsonX is integrated for advanced AI capabilities, such as image recognition and natural language processing.

### AWS (EC2 and DynamoDB)
- **AWS EC2:** Hosting the server that handles user authentication and data storage.
- **AWS DynamoDB:** A NoSQL database service used for storing user data, notes, and shared information.

### Git and Git LFS
- **Git:** Version control for managing project changes and collaboration.
- **Git LFS:** Handles large files such as 3D models and textures to ensure efficient version control.

## Setup Guide

### Prerequisites
- Unity Hub and Unity Editor (version 2022.3.28f1 or later recommended)
- Android SDK and NDK
- Xcode for iOS development
- Git and Git LFS

### Cloning the Repository

   ```sh
   git clone https://github.com/dhruvds12/ibm-next-gen-documentation.git
   ```
## Installation and Setup

### Install Git LFS and Pull Large Files

```sh
git lfs install
git lfs pull
```
## Setting Up Unity Project

1. Open Unity Hub, click on `Open`, and navigate to the cloned repository.
2. Ensure all necessary packages (ARFoundation, ARCore, etc.) are installed. If not, add them via the Unity Package Manager.

### Building for Android

1. Go to `File > Build Settings`.
2. Select `Android` and click `Switch Platform`.
3. Ensure the correct SDK and NDK paths are set in `Preferences > External Tools`.
4. Click `Build and Run`.

### Building for iOS

1. Go to `File > Build Settings`.
2. Select `iOS` and click `Switch Platform`.
3. Open Xcode and ensure all signing settings are correctly configured.
4. Click `Build and Run`.

## Usage Guide

### Main Features

- **Image Tracking:** Uses ARFoundation to place 3D models over recognized images.
- **Object Interaction:** Allows users to interact with placed 3D models, displaying detailed information.
- **Sharing Notes:** Users can share notes with others, leveraging AWS DynamoDB for data storage.

### Adding New 3D Models

1. Import your `.blend` file into Unity by dragging it into the `Assets` folder.
2. Ensure the animations and textures are correctly set up (refer to the [video guide](https://youtu.be/UQGMsL8jXRI?si=7VdJwApbn4tWMy4y&t=170)).
3. Add box colliders to each component of the model for interaction.
4. Tag each component appropriately (see Tag Convention below).
5. Create a prefab from the model and add it to the `Ar Prefab` list in the `Image Tracker With Object Manipulation` script.

### Tag Convention

- Tags must match the name of the prefab for easy identification.
- Tags for info popups should end with "Info" (e.g., ComponentInfo) to ensure proper handling.

### Managing AR Interaction

- **Enabling Detailed Interaction:** Toggle the interaction setting to enable or disable detailed interactions.
- **Navigating Information:** Use the Next and Previous buttons to navigate through the information provided for each component.

## API and Server Configuration

### Server Endpoints

- `/signup`: Handles user registration.
- `/login`: Manages user authentication.
- `/notes`: Allows users to post and retrieve notes.
- `/share`: Facilitates sharing notes with other users.
- `/sharedNotes`: Retrieves notes shared with the user.
- `/users`: Provides a list of all registered users.

### Setting Up AWS DynamoDB

Ensure your DynamoDB table is set up with the following structure:

- **Table Name:** UserNotes
- **Primary Key:** userId
- **Global Secondary Index:** username-index for efficient querying by username.

## Contributing

Please ensure you follow the standard Git workflow:

1. Fork the repository.
2. Create a new branch (`git checkout feature-branch`).
3. Commit your changes (`git commit 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Create a new Pull Request.

