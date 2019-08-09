using UnityEngine;

namespace Assets.Scripts.Terrain {

	public static class GridDef {

		private enum MeshType {
			
			Quad,
			Corner,
			Edge,
			Full

		}

		public static int Y;
		public static int[] Types;
		public static int[][] MeshColors;
		public static int[] MeshTypes;
		public static int[] MeshRotations;

		#region Constants
		private static readonly int[][] NeighborhoodQ = {
			new[] { 0 },
			new[] { 1 },
			new[] { 2 },
			new[] { 3 }
		};

		private static readonly int[][][] NeighborhoodC = {
			new[] { // Rotation 0
				new[] { 0, 1, 2 },
				new[] { 0, 1, 2, 4, 5 },
				new[] { 0, 1, 2, 3, 4 },
				new[] { 2, 3, 4 },
				new[] { 1, 2, 3, 4, 5 },
				new[] { 1, 4, 5 }
			},
			new[] { // Rotation 1
				new[] { 0, 1, 3 },
				new[] { 0, 1, 2, 3, 5 },
				new[] { 1, 2, 5 },
				new[] { 0, 1, 3, 4, 5 },
				new[] { 3, 4, 5 },
				new[] { 1, 2, 3, 4, 5 }
			},
			new[] { // Rotation 2
				new[] { 0, 1, 4 },
				new[] { 0, 1, 2, 3, 4 },
				new[] { 1, 2, 3 },
				new[] { 1, 2, 3, 4, 5 },
				new[] { 0, 1, 3, 4, 5 },
				new[] { 3, 4, 5 }
			},
			new[] { // Rotation 3
				new[] { 0, 1, 2, 3, 4 },
				new[] { 0, 1, 2 },
				new[] { 0, 1, 2, 4, 5 },
				new[] { 0, 3, 4 },
				new[] { 0, 2, 3, 4, 5 },
				new[] { 2, 4, 5 }
			}
		};

		private static readonly int[][][] NeighborhoodE = {
			new[] { // Vertical rotation 0 2
				new[] { 0, 1, 2, 3 },
				new[] { 0, 1, 2, 3 },
				new[] { 0, 1, 2, 3, 4, 5 },
				new[] { 0, 1, 2, 3, 4, 5 },
				new[] { 2, 3, 4, 5 },
				new[] { 2, 3, 4, 5 }
			},
			new[] { // Horizontal rotation 1 3
				new[] { 0, 1, 3, 4 },
				new[] { 0, 1, 2, 3, 4, 5 },
				new[] { 1, 2, 4, 5 },
				new[] { 0, 1, 3, 4 },
				new[] { 0, 1, 2, 3, 4, 5 },
				new[] { 1, 2, 4, 5 }
			},
		};

		private static readonly int[][] NeighborhoodF = {
			new[] { 0, 1, 3 },
			new[] { 0, 1, 2, 3, 4, 5 },
			new[] { 1, 2, 5 },
			new[] { 0, 1, 3, 4, 6, 7 },
			new[] { 1, 3, 4, 5, 7 },
			new[] { 1, 2, 4, 5, 7, 8 },
			new[] { 3, 6, 7 },
			new[] { 3, 4, 5, 6, 7, 8 },
			new[] { 5, 7, 8 }
		};

		private static readonly int[] Corners = { 6, 0, 2, 8 };
		private static readonly int[] Edges = { 7, 3, 1, 5};
		private static readonly int[] EdgeOtherCorner = { 8, 6, 0, 2 };
		private static readonly int[] MaskMul = { 1, 2, 4, 8 };
		#endregion

		public static void Initialize (int height, int[] types) {
			Y = height;
			int yy = Y * Y;
			MeshTypes = new int[yy];
			MeshRotations = new int[yy];
			MeshColors = new int[yy][];
			Types = types;
			
			for (int y = 0; y < Y; y++) {
				for (int x = 0; x < Y; x++) {
					int i = x + y * Y;
					int t = Types[i];

					int[] neighbors = {
						TypeAt(t, x - 1, y - 1),
						TypeAt(t, x, y - 1),
						TypeAt(t, x + 1, y - 1),
						TypeAt(t, x - 1, y),
						t, 
						TypeAt(t, x + 1, y),
						TypeAt(t, x - 1, y + 1),
						TypeAt(t, x, y + 1),
						TypeAt(t, x + 1, y + 1)
					};

					SetProperties(neighbors, out MeshType mt, out int mr, out int[] colors);
					MeshTypes[i] = (int) mt;
					MeshRotations[i] = mr;
					MeshColors[i] = colors;
				}
			}
		}

		public static int[] Neighborhood (int type, int index, int rotation) {
			switch ((MeshType) type) {
				case MeshType.Quad:
					return NeighborhoodQ[index];
				case MeshType.Corner:
					return NeighborhoodC[rotation][index];
				case MeshType.Edge:
					switch (rotation) {
						case 1:
						case 3:
							return NeighborhoodE[1][index];
						default:
							return NeighborhoodE[0][index];
					}
				default:
					return NeighborhoodF[index];
			}
		}

		private static void SetProperties (int[] neighbors, out MeshType type, out int rotation, out int[] colors) {
			int t = neighbors[4];
			int[] n = neighbors;

			int cornerCount = 0;
			int edgeCount = 0;
			int cornerMask = 0;
			int edgeMask = 0;

			for (int j = 0; j < 4; ++j) {
				int cType = n[Corners[j]];
				int eType = n[Edges[j]];
				int c2Type = n[EdgeOtherCorner[j]];
				bool addedCorner = false;

				if (cType != -1 && cType != t) {
					++cornerCount;
					cornerMask += MaskMul[j];
					addedCorner = true;
				}

				if (eType == -1 || eType == t) {
					continue;
				}

				++edgeCount;
				edgeMask += MaskMul[j];

				if (addedCorner) {
					if (cType == eType) {
						--cornerCount;
						cornerMask -= MaskMul[j];
					}
				}

				if (c2Type == eType) {
					--cornerCount;
					cornerMask -= MaskMul[(j + 1) % 4];
				}
			}

			MeshType mt;

			switch (cornerCount) {
				case 0 when edgeCount == 0:
					mt = MeshType.Quad;
					break;
				case 1 when edgeCount == 0:
					mt = MeshType.Corner;
					break;
				case 0 when edgeCount == 1:
					mt = MeshType.Edge;
					break;
				default:
					mt = MeshType.Full;
					break;
			}
			
			int mr;
			int[] mc;

			switch (mt) {
				case MeshType.Quad:
					mr = 0;
					mc = new[] { t, t, t, t };
					break;
				case MeshType.Corner:
					mr = Rotation(cornerMask);

					switch (mr) {
						case 1:
							mc = new[] {
								n[0], t, t, t, t, t
							};
							break;
						case 2:
							mc = new[] {
								t, t, n[2], t, t, t
							};
							break;
						case 3:
							mc = new[] {
								t, t, t, t, t, n[8]
							};
							break;
						default:
							mc = new[] {
								t, t, t, n[6], t, t
							};
							break;
					}
					break;
				case MeshType.Edge:
					mr = Rotation(edgeMask);

					switch (mr) {
						case 1:
							int n3 = n[3];
							mc = new[] {
								n3, t, t, n3, t, t
							};
							break;
						case 2:
							int n1 = n[1];
							mc = new[] {
								n1, n1, t, t, t, t
							};
							break;
						case 3:
							int n5 = n[5];
							mc = new[] {
								t, t, n5, t, t, n5
							};
							break;
						default:
							int n7 = n[7];
							mc = new[] {
								t, t, t, t, n7, n7
							};
							break;
					}
					break;
				default:
					mr = 0;
					n[0] = Max(n[0], n[1], n[3]);
					n[2] = Max(n[1], n[2], n[5]);
					n[6] = Max(n[3], n[6], n[7]);
					n[8] = Max(n[5], n[7], n[8]);
					mc = n;
					break;
			}

			type = mt;
			rotation = mr;
			colors = mc;
		}

		private static int Max (int a, int b, int c) {
			return Mathf.Max(a, Mathf.Max(b, c));
		}

		private static int TypeAt (int t, int x, int y) {
			if (x < 0 || x >= Y || y < 0 || y >= Y) {
				return -1;
			}

			int type = Types[x + y * Y];
			return t > type ? t : type;
		}

		private static int Rotation (int mask) {
			switch (mask) {
				case 1: return 0;
				case 2: return 1;
				case 4: return 2;
				case 8: return 3;
				default:
					Debug.Log("Invalid mask: " + mask);
					return 0;
			}
		}

	}

}