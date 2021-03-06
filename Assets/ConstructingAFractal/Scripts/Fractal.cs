﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour 
{
	#region Inspector Fields

	[SerializeField]
	private Mesh [] meshes;

	[SerializeField]
	private Material material;

	[SerializeField]
	private int maxDepth;

	[SerializeField]
	private float childScale;

	[SerializeField]
	private float spawnProbability;

	[SerializeField]
	private float maxRotationSpeed;

	[SerializeField]
	private float maxTwist;

	#endregion

	#region Private Fields

	private int depth = 0;
	private static Vector3[] childDirections = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
	private static Quaternion[] childOrientations = { Quaternion.identity, Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f), Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f) };
	private Material[,] materials;
	private float rotationSpeed;

	#endregion

	private void Start()
	{
		rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

		if(materials == null)
		{
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];
		if(depth < maxDepth)
		{
			StartCoroutine(CreateChildren());
		}
	}

	private void Update()
	{
		transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}

	private void Initialize(Fractal parent, int childIndex)
	{
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
		transform.parent = parent.transform;
		//All children will be scaled down to (0.5f, 0.5f, 0.5f), but since
		//each object created is a child of the previous object. it also gets
		//scaled down since the parent was scaled, so it gets scaled down even
		//further.
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		//orientation is used so the children are not all upright like the parent.
		transform.localRotation = childOrientations[childIndex];
	}

	private IEnumerator CreateChildren()
	{
		for(int index = 0; index < childDirections.Length; index++)
		{
			//random.value produces a random value between zero and one.
			if(Random.value < spawnProbability)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
				new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, index);
			}
		}
	}

	private void InitializeMaterials()
	{
		materials = new Material[maxDepth + 1, 2];
		for(int i = 0; i <= maxDepth; i++)
		{
			float t = (float)i / maxDepth;
			t *= t;
			materials[i, 0] = new Material(material);
			materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
		}
		materials[maxDepth, 0].color = Color.magenta;
		materials[maxDepth, 1].color = Color.red;
	}
}
