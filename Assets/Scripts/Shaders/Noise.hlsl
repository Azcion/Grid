#ifndef NOISE_INCLUDED
#define NOISE_INCLUDED

half rand (half2 uv) {
	const half2 other = half2(12.9898, 78.233);
	const half multiplier = 43758.5453;

	return frac(sin(dot(uv, other)) * multiplier);
}

half value (half2 uv) {
	half2 i = floor(uv);
	half2 f = frac(uv);

	const half2 plusX = half2(1, 0);
	const half2 plusY = half2(0, 1);
	const half2 plusXY = half2(1, 1);

	half tl = rand(i);
	half tr = rand(i + plusX);
	half bl = rand(i + plusY);
	half br = rand(i + plusXY);
	half2 q = f * f * (3 - 2 * f);
	half top = lerp(tl, tr, q.x);
	half btm = lerp(bl, br, q.x);
	half mix = lerp(top, btm, q.y);

	return mix;
}

half valueFBM4 (half2 uv) {
	half result = 0;
	half scale = .5;

	for (int i = 0; i < 4; ++i) {
		result += value(uv) * scale;
		uv *= 2;
		scale *= .5;
	}

	return result;
}

half valueFBM2 (half2 uv) {
	half result = 0;
	half scale = .5;

	for (int i = 0; i < 2; ++i) {
		result += value(uv) * scale;
		uv *= 2;
		scale *= .5;
	}

	return result;
}

#endif // NOISE_INCLUDED